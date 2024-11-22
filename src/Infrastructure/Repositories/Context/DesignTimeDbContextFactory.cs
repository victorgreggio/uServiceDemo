using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace uServiceDemo.Infrastructure.Repositories.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WeatherForecastDbContext>
{
    public WeatherForecastDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<WeatherForecastDbContext>();
        builder.UseNpgsql("Host=localhost;Database=weather_forecast;Username=weather_forecast;Password=P@ssw0rd");
        return new WeatherForecastDbContext(builder.Options);
    }
}