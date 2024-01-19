using System.Globalization;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Localization;
using Serilog;
using WebApiTemplate.Security;

namespace WebApiTemplate.Bootstrap;

public static class WebAppBuilderExtensions
{
    public static WebApplicationBuilder AddWebApiFeatures(this WebApplicationBuilder builder)
    {
        builder.WebHost
            .ConfigureKestrel(options =>
            {
                options.AddServerHeader = false;
                options.AllowSynchronousIO = false;
            })
            .CaptureStartupErrors(true);
        builder.Host.UseConsoleLifetime(options => options.SuppressStatusMessages = true);
        builder.Services.AddFastEndpoints();

        return builder;
    }

    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        return builder;
    }

    public static WebApplicationBuilder AddCorsUrls(this WebApplicationBuilder builder)
    {
        var securityOptions = builder.Configuration
            .GetSection(SecurityConfigurationOptions.ConfigurationSectionName)
            .Get<SecurityConfigurationOptions>();

#pragma warning disable IDE0270 // Use coalesce expression
        if (securityOptions == null)
        {
            throw new Exception(
                $"Missing \"{SecurityConfigurationOptions.ConfigurationSectionName:D}\" configuration section or its contents are wrong.");
        }
#pragma warning restore IDE0270 // Use coalesce expression

        if (securityOptions.Cors.Origins == null || securityOptions.Cors.Origins.Count == 0)
        {
            throw new Exception(
                $"Missing CORS URLs in \"{SecurityConfigurationOptions.ConfigurationSectionName:D}\" configuration section.");
        }

        builder.Services.AddCors(options => options.AddDefaultPolicy(
            corsPolicyBuilder => corsPolicyBuilder
                .WithOrigins([.. securityOptions.Cors.Origins])
                .WithExposedHeaders("Content-Disposition")
                .AllowAnyHeader()
                .AllowAnyMethod()));

        return builder;
    }

    public static WebApplicationBuilder AddRequestLocalization(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new("en"),
                new("lv"),
                new("nb-NO"),
                new("ru"),
            };

            options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
        });

        return builder;
    }

    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
            loggerConfiguration
                .ReadFrom.Configuration(hostBuilderContext.Configuration)
                .Enrich.FromLogContext());
        return builder;
    }

    public static WebApplicationBuilder AddApplicationInsights(this WebApplicationBuilder builder)
    {
        // TODO: remove this options setting after Serilog.Sinks.ApplicationInsights fixes usage of deprecated Telemetryconfiguration.Active singleton instance.
        // More info: https://github.com/serilog/serilog-sinks-applicationinsights/issues/156
        builder.Services.AddApplicationInsightsTelemetry(options => options.EnableActiveTelemetryConfigurationSetup = true);
#if DEBUG
        // Disable line below to get Debug events in View->Other Windows->AppInsights Search (Note: It also will spam Debug Output window)
        Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryDebugWriter.IsTracingDisabled = true;
#endif
        return builder;
    }

    public static WebApplicationBuilder AddSwaggerServices(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.SwaggerDocument(
                settings =>
                {
                    settings.DocumentSettings = docCfg =>
                    {
                        docCfg.Title = "WebApiTemplate";
                        docCfg.Version = "v1.0";
                        docCfg.DocumentName = "v1.0";
                        docCfg.Description = "Here goes a longer and detailed description on API purpose and usage.";
                        docCfg.SchemaSettings.GenerateEnumMappingDescription = true;
                    };
                    settings.EndpointFilter = EndpointDocumentationFilter;
                    settings.AutoTagPathSegmentIndex = 0;
                    settings.ShortSchemaNames = true;
                });
        }

        return builder;
    }

    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddDependencies(builder.Configuration);
        return builder;
    }

    /// <summary>
    /// Filters out which Endpoints appear in Swagger OpenApi documentation.
    /// </summary>
    /// <param name="endpoint">endpoint description</param>
    /// <returns>True - appears, False - does not appear.</returns>
    private static bool EndpointDocumentationFilter(EndpointDefinition endpoint) =>
        endpoint.EndpointTags?.Contains("Pages") != true;
}
