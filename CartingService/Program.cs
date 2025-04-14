using Amazon.SQS;
using CartingService.BLL;
using CartingService.BLL.Interfaces;
using CartingService.DAL;
using CartingService.DAL.Interfaces;
using CartingService.Listeners;
using CartingService.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register the IUnitOfWork and ICartBL services
builder.Services.AddScoped<IUnitOfWork>(sp => new LiteDBUnitOfWork("Filename=CartingService.db;Connection=shared"));
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

    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.OAuth2,
        Flows = new Microsoft.OpenApi.Models.OpenApiOAuthFlows
        {
            Password = new Microsoft.OpenApi.Models.OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:7051/connect/token"),
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
        options.Authority = "https://localhost:7051";
        options.RequireHttpsMetadata = false;
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
if (app.Environment.IsDevelopment())
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
app.UseAuthentication();
app.UseMiddleware<CartingService.Middleware.TokenLoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
