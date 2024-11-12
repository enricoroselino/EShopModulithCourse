using Carter;
using DotNetEnv;
using EShopModulithCourse.Server.Configurations;
using EShopModulithCourse.Server.Shared.Exceptions;
using EShopModulithCourse.Server.Shared.Extensions;
using EShopModulithCourse.Server.Shared.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

builder.Services
    .AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>()
    .AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

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

await app.SeedDatabaseAsync();

app
    .UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
    .UseExceptionHandler(cfg => { });

app.UseHttpsRedirection();
app.MapCarter();
await app.RunAsync();