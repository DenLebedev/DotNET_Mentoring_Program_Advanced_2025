using CatalogService.Application.Mappings;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CatalogService.Application.Services;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Repositories;
using CatalogService.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));

// Add Application & Infrastructure dependencies
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add services for HATEOAS
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x => x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34))
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }