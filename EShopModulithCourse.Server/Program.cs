using Carter;
using DotNetEnv;
using EShopModulithCourse.Server.Configurations;
using EShopModulithCourse.Server.Shared.Exceptions;
using Modules.Basket;
using Modules.Catalog;
using Modules.Ordering;

Env.Load(".env");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSerilogConfig();
builder.Services
    .AddCors()
    .AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandler>();

builder.Services
    .AddBasketModule()
    .AddCatalogModule()
    .AddOrderingModule();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();

app
    .UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
    .UseExceptionHandler(cfg => { });

app.UseHttpsRedirection();
app.MapCarter();
await app.RunAsync();