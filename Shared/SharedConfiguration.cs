using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configurations;
using Shared.Data.Interceptors;
using Shared.Exceptions;
using Shared.Providers;

namespace Shared;

public static class SharedConfiguration
{
    public static IServiceCollection AddSharedConfiguration(this IServiceCollection services)
    {
        services.AddSerilogConfig();

        services
            .AddCors()
            .AddProblemDetails()
            .AddExceptionHandler<GlobalExceptionHandler>();

        services
            .AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>()
            .AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services
            .AddScoped<IUuidProvider, UuidProvider>();

        return services;
    }

    public static IApplicationBuilder UseSharedConfiguration(this IApplicationBuilder app)
    {
        app
            .UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
            .UseExceptionHandler(cfg => { });

        return app;
    }
}