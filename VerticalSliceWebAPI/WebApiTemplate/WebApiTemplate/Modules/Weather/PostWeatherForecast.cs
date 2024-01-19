using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using WebApiTemplate.Connectors.Database;
using WebApiTemplate.Connectors.Database.Entities;

namespace WebApiTemplate.Modules.Weather;

[UsedImplicitly]
public class PostWeatherForecast(PostWeatherForecastHandler handler): Endpoint<PostWeatherForecastRequest>
{
    public override void Configure()
    {
        // Endpoint setup (behaviour)
        Post(WeatherUrls.Weather);
        Tags(WeatherUrls.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(WeatherUrls.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Save new weather data";
            swagger.Description = "Saves new weather data to DB";
            swagger.ExampleRequest = new PostWeatherForecastRequest
                { Date = new DateOnly(2023, 1, 12), TemperatureC = 12, Summary = "Chilly" };
        });
    }

    public override async Task HandleAsync(PostWeatherForecastRequest req, CancellationToken cancellationToken)
    {
        await handler.Handle(req, cancellationToken);
        await SendOkAsync(cancellationToken);
    }
}

/// <summary>
/// Weather data for specified date.
/// </summary>
[ExcludeFromCodeCoverage]
public class PostWeatherForecastRequest
{
    /// <summary>
    /// Date of weather.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Temperature in celsius (C).
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Description of weather.
    /// </summary>
    public string? Summary { get; set; }
}

[UsedImplicitly]
public class PostWeatherForecastHandler(WebApiTemplateDbContext webApiTemplateDbContext)
{
    public async Task Handle(PostWeatherForecastRequest req, CancellationToken cancellationToken)
    {
        await webApiTemplateDbContext.WeatherForecasts.AddAsync(
            new WeatherForecastRecord
                {
                    Date = req.Date,
                    Summary = req.Summary,
                    TemperatureC = req.TemperatureC
                },
            cancellationToken);

        await webApiTemplateDbContext.SaveChangesAsync(cancellationToken);
    }
}
