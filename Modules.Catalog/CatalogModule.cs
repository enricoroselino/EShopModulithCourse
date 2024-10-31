using EShopModulithCourse.Server.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        var assembly = typeof(CatalogModule).Assembly;
        services.AddMediatorFromAssemblies(assembly);
        services.AddCarterFromAssemblies(assembly);
        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        return app;
    }
}