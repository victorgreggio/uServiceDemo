using AGTec.Common.Repository;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Infrastructure.Repositories.Context;

namespace uServiceDemo.Infrastructure.Repositories;

public interface
    IWeatherForecastReadOnlyRepository : IReadOnlyRepository<WeatherForecastEntity, WeatherForecastDbContext>
{
}