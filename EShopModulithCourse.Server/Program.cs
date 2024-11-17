using DotNetEnv;
using Modules.Basket;
using Modules.Catalog;
using Modules.Ordering;
using Modules.Shared;
using Shared;

Env.Load(".env");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .AddModulesConfiguration()
    .AddSharedConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseModulesConfigurations();
app.UseSharedConfiguration();
await app.RunAsync();