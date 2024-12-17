using Chapter6CustomMiddleware.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Chapter6CustomMiddleware.ExtensionMethods;

/// <summary>
/// Example for page 20
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomLogger
            (this IServiceCollection services)
    {
        // Retrieve LoggerConfig from IConfiguration
        var config = new LoggerConfig();
        var configuration = services.BuildServiceProvider()
                    .GetRequiredService<IConfiguration>();
        configuration.GetSection("LoggerConfig").Bind(config);
        // Register CustomLogger with the configuration
        services.AddSingleton<CustomLogger>
            (provider => new CustomLogger(config));
        return services;
    }
}

