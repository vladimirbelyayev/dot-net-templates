using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Connectors.Database;

namespace WebApiTemplate.Modules.Weather;

[UsedImplicitly]
public class GetWeatherForecast(GetWeatherForecastHandler handler): EndpointWithoutRequest<IEnumerable<GetWeatherForecastResponse>>
{
    public override void Configure()
    {
        // Endpoint setup (behaviour)
        Get(WeatherUrls.Weather);
        Tags(WeatherUrls.SwaggerTag);
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags(WeatherUrls.SwaggerTag));
        Summary(swagger =>
        {
            swagger.Summary = "Classic VS template sample of weather data.";
            swagger.Description = "Returns list of objects with weather data from DB.";
            swagger.Response<GetWeatherForecastResponse>((int)HttpStatusCode.OK, "Returns short list of weather data.");
            swagger.ResponseExamples[200] = new GetWeatherForecastResponse
                { Date = new DateOnly(2023, 1, 12), TemperatureC = 12, Summary = "Chilly" };
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(await handler.Handle(cancellationToken), cancellationToken);
}

/// <summary>
/// Weather data for specified date.
/// </summary>
[ExcludeFromCodeCoverage]
public class GetWeatherForecastResponse
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
    /// Temperature in Farenheiot (F).
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Description of weather.
    /// </summary>
    public string? Summary { get; set; }
}

[UsedImplicitly]
public class GetWeatherForecastHandler(WebApiTemplateDbContext dbContext)
{
    public async Task<IEnumerable<GetWeatherForecastResponse>> Handle(CancellationToken cancellationToken) =>
        await dbContext.WeatherForecasts.Select(x =>
            new GetWeatherForecastResponse
        {
            Date = x.Date,
            TemperatureC = x.TemperatureC,
            Summary = x.Summary
        }).ToListAsync(cancellationToken);
}
