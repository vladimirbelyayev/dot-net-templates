using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiTemplate.Connectors.Database.Entities;

namespace WebApiTemplate.Connectors.Database.EntityConfiguration;

public class WeatherForecastRecordConfiguration: IEntityTypeConfiguration<WeatherForecastRecord>
{
    public void Configure(EntityTypeBuilder<WeatherForecastRecord> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .IsRequired();
    }
}