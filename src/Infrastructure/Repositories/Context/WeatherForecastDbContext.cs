using Microsoft.EntityFrameworkCore;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Infrastructure.Repositories.Mappings;

namespace uServiceDemo.Infrastructure.Repositories.Context;

public class WeatherForecastDbContext : DbContext
{
    public WeatherForecastDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<WeatherForecastEntity> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecastEntity>(WeatherForecastEntityMap.Map);
        modelBuilder.Entity<WindEntity>(WindEntityMap.Map);
    }
}