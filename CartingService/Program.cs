using Amazon.SQS;
using CartingService.BLL;
using CartingService.BLL.Interfaces;
using CartingService.DAL;
using CartingService.DAL.Interfaces;
using CartingService.Listeners;
using CartingService.Mappings;
using CartingService.Options;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "CartingService")
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(
        new TelemetryConfiguration { ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"] },
        TelemetryConverter.Traces)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Load IdentityServer authority from configuration
var identityAuthority = builder.Configuration["Identity:Authority"];
var swaggerTokenUrl = builder.Environment.EnvironmentName == "Docker"
    ? "http://localhost:7051/connect/token"
    : $"{identityAuthority}/connect/token";

// Configure LiteDb connection
builder.Services.Configure<LiteDbOptions>(builder.Configuration.GetSection("LiteDbOptions"));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddGrpc();

// Register the IUnitOfWork and ICartBL services
builder.Services.AddScoped<IUnitOfWork>(sp =>
{
    var options = sp.GetRequiredService<IOptions<LiteDbOptions>>().Value;
    return new LiteDBUnitOfWork($"Filename={options.DatabasePath};Connection=shared");
});
builder.Services.AddScoped<ICartBL, CartBL>();

// Add AWS SQS support
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddHostedService<CatalogItemUpdateListener>();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

// Add Versioned API Explorer 
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add SwaggerGen
builder.Services.AddSwaggerGen(options =>
{
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        var groupName = apiDesc.GroupName;
        return !string.IsNullOrEmpty(groupName) && groupName == docName;
    });

    options.OperationFilter<SwaggerDefaultValues>();
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri(swaggerTokenUrl),
                Scopes = new Dictionary<string, string>
                {
                    { "carting_api", "Access to Carting API" },
                    { "openid", "OpenID Connect" },
                    { "profile", "User Profile" },
                    { "offline_access", "Refresh Token" }
                }
            }
        }
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "carting_api" }
        }
    });
});

// Register Swagger options config
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// Add JWT authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = identityAuthority;
        options.RequireHttpsMetadata = identityAuthority.StartsWith("https://");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomerOrManager", policy =>
        policy.RequireRole("Manager", "StoreCustomer"));
});

var app = builder.Build();

// Get the version description provider
var provider = app.Services.GetRequiredService<Microsoft.AspNetCore.Mvc.ApiExplorer.IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{desc.GroupName}/swagger.json",
                $"CartingService API {desc.GroupName.ToUpperInvariant()}");
        }

        options.OAuthClientId("swagger-ui");
        options.OAuthClientSecret("swagger-secret");
        options.OAuthAppName("CartingService Swagger UI");
        options.OAuthUsePkce();
    });

    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger")) await next();
    else await next();
});
app.UseAuthentication();
app.UseMiddleware<CartingService.Middleware.TokenLoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<CartingService.GrpcServices.CartServiceGrpc>();
app.MapGet("/", () => "This server hosts a gRPC CartingService. Use a gRPC client such as BloomRPC to interact with it.");

app.Run();

public partial class Program { }
