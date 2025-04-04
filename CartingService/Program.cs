using CartingService.BLL.Interfaces;
using CartingService.BLL;
using CartingService.DAL;
using CartingService.DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the IUnitOfWork and ICartBL services
builder.Services.AddScoped<IUnitOfWork>(sp => new LiteDBUnitOfWork("Filename=CartingService.db;Connection=shared"));
builder.Services.AddScoped<ICartBL, CartBL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }