using AGTec.Common.Repository;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Infrastructure.Repositories.Context;

namespace uServiceDemo.Infrastructure.Repositories;

internal class WeatherForecastReadOnlyRepository : ReadOnlyRepository<WeatherForecastEntity, WeatherForecastDbContext>,
    IWeatherForecastReadOnlyRepository
{
    public WeatherForecastReadOnlyRepository(WeatherForecastDbContext context)
        : base(context)
    {
    }
}