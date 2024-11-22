using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Infrastructure.Repositories.Mappings;

internal static class WeatherForecastEntityMap
{
    public static void Map(EntityTypeBuilder<WeatherForecastEntity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable(Constants.Database.Tables.WeatherForecastTable);

        entityTypeBuilder.HasKey(x => x.Id);
        entityTypeBuilder.Property(x => x.Created).ValueGeneratedOnAdd().HasDefaultValueSql("timezone('UTC', now())");
        entityTypeBuilder.Property(x => x.LastUpdated).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("timezone('UTC', now())");
        entityTypeBuilder.Property(x => x.Version).IsRowVersion();
    }
}