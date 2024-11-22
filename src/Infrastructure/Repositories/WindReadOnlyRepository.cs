using AGTec.Common.Repository;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Infrastructure.Repositories.Context;

namespace uServiceDemo.Infrastructure.Repositories;

internal class WindReadOnlyRepository : ReadOnlyRepository<WindEntity, WeatherForecastDbContext>,
    IWindReadOnlyRepository
{
    public WindReadOnlyRepository(WeatherForecastDbContext context)
        : base(context)
    {
    }
}