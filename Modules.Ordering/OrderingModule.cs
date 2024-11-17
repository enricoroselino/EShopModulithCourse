using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;

namespace Modules.Ordering;

public static class OrderingModule
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services)
    {
        var assembly = typeof(OrderingModule).Assembly;
        services
            .AddMediatorFromAssemblies(assembly)
            .AddCarterFromAssemblies(assembly);
        
        return services;
    }

    public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
    {
        return app;
    }
}