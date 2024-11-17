using MassTransit;

namespace Shared.Infrastructure.Messaging;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitFromAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        // cant be called multiple times, so pass all the assemblies at once
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.SetInMemorySagaRepositoryProvider();

            cfg.AddConsumers(assemblies);
            cfg.AddSagaStateMachines(assemblies);
            cfg.AddSagas(assemblies);
            cfg.AddActivities(assemblies);

            cfg.UsingInMemory((context, configurator) => { configurator.ConfigureEndpoints(context); });
        });

        return services;
    }
}