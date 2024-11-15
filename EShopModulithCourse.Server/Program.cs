using DotNetEnv;
using Modules.Basket;
using Modules.Catalog;
using Modules.Ordering;
using Shared;

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

app.UseHttpsRedirection();
app.UseSharedConfiguration();

await app.RunAsync();