using Microsoft.Extensions.Options;

namespace WebApiTemplate.Bootstrap;

public static class WebAppExtensions
{
    public static WebApplication UseWebApiFeatures(this WebApplication app)
    {
        app.UseFastEndpoints(config =>
        {
            config.Endpoints.ShortNames = true;
        });

        return app;
    }

    public static WebApplication UseCORS(this WebApplication app)
    {
        app.UseCors();
        return app;
    }

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication UseRequestLocalization(this WebApplication app)
    {
        var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>()
                      ?? throw new Exception(
                          "UseRequestLocalization was not able to get RequestLocalizationOptions. Did you forget to use AddRequestLocalization() to services?");

        app.UseRequestLocalization(options.Value);
        return app;
    }

    public static WebApplication UseSwaggerPage(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi(settings =>
            {
                settings.AdditionalSettings["filter"] = true;
                settings.AdditionalSettings["persistAuthorization"] = true;
                settings.AdditionalSettings["displayRequestDuration"] = true;
                settings.AdditionalSettings["tryItOutEnabled"] = true;
                settings.TagsSorter = "alpha";
                settings.OperationsSorter = "alpha";
                settings.CustomInlineStyles = """
                                              .servers-title, .servers{display:none}
                                              .swagger-ui .info{margin:10px 0}
                                              .swagger-ui .scheme-container{margin:10px 0;padding:10px 0}
                                              .swagger-ui .info .title{font-size:25px}
                                              .swagger-ui textarea{min-height:150px}
                                              """;
            });
        }

        return app;
    }
}