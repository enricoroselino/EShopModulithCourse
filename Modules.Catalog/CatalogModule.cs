﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Data;
using Modules.Catalog.Data.Seeders;
using Shared.Extensions;
using Shared.Infrastructure.Messaging;
using Shared.Interfaces;

namespace Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        services.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();
            options.AddInterceptors(interceptors);
        });

        services.AddScoped<IDataSeeder, CatalogSeeder>();
        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        app.MigrateDatabaseAsync<CatalogDbContext>().GetAwaiter().GetResult();
        return app;
    }
}