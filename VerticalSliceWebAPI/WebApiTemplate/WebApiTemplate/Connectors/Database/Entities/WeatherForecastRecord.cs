namespace WebApiTemplate.Connectors.Database.Entities
{
    public class WeatherForecastRecord
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public string? Summary { get; set; }
    }
}