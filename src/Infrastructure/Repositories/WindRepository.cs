using AGTec.Common.Repository;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Infrastructure.Repositories.Context;

namespace uServiceDemo.Infrastructure.Repositories;

internal class WindRepository : Repository<WindEntity, WeatherForecastDbContext>, IWindRepository
{
    public WindRepository(WeatherForecastDbContext context)
        : base(context)
    {
    }
}