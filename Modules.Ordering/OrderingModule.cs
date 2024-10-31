using EShopModulithCourse.Server.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Ordering;

public static class OrderingModule
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services)
    {
        var assembly = typeof(OrderingModule).Assembly;
        services.AddMediatorFromAssemblies(assembly);
        services.AddCarterFromAssemblies(assembly);
        return services;
    }

    public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
    {
        return app;
    }
}