using Microsoft.OpenApi.Models;
using CartingService.BLL.Interfaces;
using CartingService.BLL;
using CartingService.DAL;
using CartingService.DAL.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;
using CartingService.Mappings;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container
// builder.Services.AddControllers()
//     .AddApplicationPart(typeof(CartingService.Controllers.V1.CartV1Controller).Assembly)
//     .AddApplicationPart(typeof(CartingService.Controllers.V2.CartV2Controller).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register the IUnitOfWork and ICartBL services
builder.Services.AddScoped<IUnitOfWork>(sp => new LiteDBUnitOfWork("Filename=CartingService.db;Connection=shared"));
builder.Services.AddScoped<ICartBL, CartBL>();

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
    // You’ll add actual docs per version below using a provider
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        var groupName = apiDesc.GroupName;
        return !string.IsNullOrEmpty(groupName) && groupName == docName;
    });

    options.OperationFilter<SwaggerDefaultValues>();

    // Required for showing correct versioning info in Swagger
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

// Register Swagger options config
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

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
    });

    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }