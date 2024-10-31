using EShopModulithCourse.Server.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services)
    {
        var assembly = typeof(BasketModule).Assembly;
        services.AddMediatorFromAssemblies(assembly);
        services.AddCarterFromAssemblies(assembly);
        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        return app;
    }
}