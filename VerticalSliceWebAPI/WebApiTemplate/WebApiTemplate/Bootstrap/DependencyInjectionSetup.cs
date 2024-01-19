using WebApiTemplate.Connectors.Database;
using WebApiTemplate.Modules.Weather;
using WebApiTemplate.Security;

namespace WebApiTemplate.Bootstrap;

public static class DependencyInjectionSetup
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterConfigurationObjects(configuration);
        services.AddDatabaseContext(configuration);
        RegisterHandlers(services);
        return services;
    }

    private static IServiceCollection RegisterConfigurationObjects(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecurityConfigurationOptions>(
            configuration.GetSection(SecurityConfigurationOptions.ConfigurationSectionName));
        return services;
    }

    private static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        services.AddScoped<GetWeatherForecastHandler>();
        services.AddScoped<PostWeatherForecastHandler>();
        return services;
    }

    // private static IServiceCollection RegisterHandlers(this IServiceCollection services)
    // {
    //     var handlerClasses = typeof(WeatherForecastHandler).Assembly.GetExportedTypes()
    //         .Where(type =>
    //             type.Namespace?.StartsWith("WebApiTemplate.CoreLogic.Handlers", StringComparison.OrdinalIgnoreCase) == true
    //             && type.GetInterfaces().Length > 0
    //             && !type.IsEnum);
    //
    //     foreach (var classImplementation in handlerClasses)
    //     {
    //         foreach (var classInterface in classImplementation.GetInterfaces())
    //         {
    //             services.TryAddTransient(classInterface, classImplementation);
    //         }
    //     }
    //
    //     return services;
    // }
}
