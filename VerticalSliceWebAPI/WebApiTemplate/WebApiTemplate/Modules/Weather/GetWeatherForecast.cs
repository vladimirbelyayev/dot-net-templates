using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using WebApiTemplate.Connectors.Database.Entities;

namespace WebApiTemplate.Modules.Weather;

[UsedImplicitly]
public class GetWeatherForecast(WeatherForecastHandler handler): EndpointWithoutRequest<IEnumerable<GetWeatherForecastResponse>>
{
    public override void Configure()
    {
        // Endpoint setup (behaviour)
        Get("/weather");
        Tags("Weather");
        AllowAnonymous();

        // Swagger documentation
        Description(swagger => swagger
            .WithTags("Weather"));
        Summary(swagger =>
        {
            swagger.Summary = "Classic VS template sample of weather data.";
            swagger.Description = "Returns list of objects with random weather data.";
            swagger.Response<GetWeatherForecastResponse>((int)HttpStatusCode.OK, "Returns short list of weather data.");
            swagger.ResponseExamples[200] = new GetWeatherForecastResponse
                { Date = new DateOnly(2023, 1, 12), TemperatureC = 12, Summary = "Chilly" };
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) =>
        await SendOkAsync(await handler.Handle(), cancellationToken);
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
public class WeatherForecastHandler
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public async ValueTask<IEnumerable<GetWeatherForecastResponse>> Handle() =>
        await ValueTask.FromResult(Enumerable.Range(1, 5).Select(index =>
                new WeatherForecastRecord
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
            .Select(domainModel => new GetWeatherForecastResponse
            {
                Date = domainModel.Date,
                TemperatureC = domainModel.TemperatureC,
                Summary = domainModel.Summary
            })
            .ToList());
}