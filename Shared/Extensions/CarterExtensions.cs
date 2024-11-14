using System.Reflection;
using Carter;
using Microsoft.Extensions.DependencyInjection;
using Shared.Providers;

namespace Shared.Extensions;

public static class CarterExtensions
{
    public static IServiceCollection AddCarterFromAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddCarter(configurator: Configurator);
        return services;

        void Configurator(CarterConfigurator cfg)
        {
            var modules = AssembliesProvider.GetInterfaceTypes<ICarterModule>(assemblies);
            cfg.WithModules(modules);
        }
    }
}