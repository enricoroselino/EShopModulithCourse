﻿using EShopModulithCourse.Server.Shared;
using EShopModulithCourse.Server.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Data;
using Modules.Catalog.Data.Seeders;

namespace Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        var assembly = typeof(CatalogModule).Assembly;
        services.AddMediatorFromAssemblies(assembly);
        services.AddCarterFromAssemblies(assembly);

        services.AddDbContext<CatalogDbContext>();
        
        services.AddScoped<IDataSeeder, CatalogDummyDataSeeder>();
        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        app.MigrateDatabaseAsync<CatalogDbContext>().GetAwaiter().GetResult();
        return app;
    }
}