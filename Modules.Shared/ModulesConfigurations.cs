namespace Modules.Shared;

public static class ModulesConfigurations
{
    public static IServiceCollection AddModulesConfiguration(this IServiceCollection services)
    {
        var catalogAssembly = typeof(CatalogModule).Assembly;
        var basketAssembly = typeof(BasketModule).Assembly;
        var orderingAssembly = typeof(OrderingModule).Assembly;

        services
            .AddMediatorFromAssemblies(catalogAssembly, basketAssembly, orderingAssembly)
            .AddCarterFromAssemblies(catalogAssembly, basketAssembly, orderingAssembly)
            .AddMassTransitFromAssemblies(catalogAssembly, basketAssembly, orderingAssembly)
            .AddQuartzFromAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

        services
            .AddBasketModule()
            .AddCatalogModule()
            .AddOrderingModule();

        return services;
    }


    public static IApplicationBuilder UseModulesConfigurations(this IApplicationBuilder app)
    {
        app
            .UseBasketModule()
            .UseCatalogModule()
            .UseOrderingModule();

        return app;
    }
}