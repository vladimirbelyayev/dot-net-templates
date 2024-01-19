using System.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiTemplate.Connectors.Database;
using WebApiTemplate.Modules.Weather;
using WebApiTemplate.Security;

namespace WebApiTemplate.Bootstrap;

public static class DependencyInjectionSetup
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterConfigurationOptions(configuration);
        services.AddDatabaseContext(configuration);
        RegisterHandlers(services);
        return services;
    }

    private static IServiceCollection RegisterConfigurationOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SecurityConfigurationOptions>()
            .Bind(configuration.GetSection(SecurityConfigurationOptions.ConfigurationSectionName))
            //See https://learn.microsoft.com/en-us/dotnet/core/extensions/options#options-validation for validation
            .ValidateDataAnnotations();

        return services;
    }

    private static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        var handlerClasses = typeof(GetWeatherForecastHandler).Assembly.GetExportedTypes()
            .Where(type =>
                type.Namespace?.StartsWith("WebApiTemplate.Modules", StringComparison.OrdinalIgnoreCase) == true
                && type.IsClass
                && type.Name.EndsWith("Handler", StringComparison.OrdinalIgnoreCase));

        foreach (var classImplementation in handlerClasses)
        {
            services.TryAddScoped(classImplementation);
        }

        return services;
    }
}
