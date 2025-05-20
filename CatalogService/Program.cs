using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using CatalogService.Application.Events;
using CatalogService.Application.Intefaces;
using CatalogService.Application.Mappings;
using CatalogService.Application.Services;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Context;
using CatalogService.Infrastructure.Messaging;
using CatalogService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Load IdentityServer authority from configuration
var identityAuthority = builder.Configuration["Identity:Authority"];

if (string.IsNullOrEmpty(identityAuthority))
    throw new InvalidOperationException("Missing Identity:Authority configuration.");

var swaggerTokenUrl = builder.Environment.EnvironmentName == "Docker"
    ? "http://localhost:7051/connect/token"
    : $"{identityAuthority}/connect/token";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CatalogService", Version = "v1" });

    // Add OAuth2 support
    c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.OAuth2,
        Flows = new Microsoft.OpenApi.Models.OpenApiOAuthFlows
        {
            Password = new Microsoft.OpenApi.Models.OpenApiOAuthFlow
            {
                TokenUrl = new Uri(swaggerTokenUrl),
                Scopes = new Dictionary<string, string>
                {
                    { "catalog_api", "Access to Catalog API" },
                    { "offline_access", "Refresh token" },
                    { "openid", "OpenID" },
                    { "profile", "Profile" }
                }
            }
        }
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
            new[] { "catalog_api" }
        }
    });
});


builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));

// Add AWS SQS support
var awsSection = builder.Configuration.GetSection("AWS");
var serviceUrl = awsSection.GetValue<string>("ServiceURL");
var region = awsSection.GetValue<string>("Region");
var accessKey = awsSection.GetValue<string>("AccessKey");
var secretKey = awsSection.GetValue<string>("SecretKey");

if (!string.IsNullOrEmpty(serviceUrl))
{
    // Local ElasticMQ in Docker
    var sqsConfig = new AmazonSQSConfig
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(region),
        ServiceURL = serviceUrl,
        UseHttp = true
    };

    var credentials = new BasicAWSCredentials(accessKey, secretKey);
    builder.Services.AddSingleton<IAmazonSQS>(new AmazonSQSClient(credentials, sqsConfig));
}
else
{
    // Real AWS environment (IAM, EC2, profile, etc.)
    builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
    builder.Services.AddAWSService<IAmazonSQS>();
}

// Dependency Injection for Event Publisher
builder.Services.AddScoped<IEventPublisher, SqsPublisher>();

// Add Application & Infrastructure dependencies
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
});

// Add services for HATEOAS
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x =>
{
    var actionContextAccessor = x.GetRequiredService<IActionContextAccessor>();
    if (actionContextAccessor.ActionContext == null)
    {
        throw new InvalidOperationException("ActionContext cannot be null.");
    }
    return x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(actionContextAccessor.ActionContext);
});

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatalogService v1");

        c.OAuthClientId("swagger-ui");
        c.OAuthClientSecret("swagger-secret");
        c.OAuthAppName("CatalogService Swagger");
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
