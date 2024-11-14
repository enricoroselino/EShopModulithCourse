using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Shared.Configurations;

internal static class SerilogConfiguration
{
    public static IServiceCollection AddSerilogConfig(this IServiceCollection services)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console();

        Log.Logger = loggerConfiguration.CreateLogger();
        services.AddSerilog(Log.Logger);
        return services;
    }
}