using Carter;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Modules.Basket;
using Modules.Catalog;
using Modules.Ordering;
using Shared;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Exceptions;
using Shared.Extensions;

Env.Load(".env");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .AddSharedConfiguration();

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

await app.SeedDatabaseAsync();

app.UseHttpsRedirection();
app.MapCarter();
await app.RunAsync();