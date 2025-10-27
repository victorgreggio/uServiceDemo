using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGTec.Common.CQRS.QueryHandlers;
using Microsoft.EntityFrameworkCore;
using uServiceDemo.Application.Queries;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Infrastructure.Repositories;

namespace uServiceDemo.Application.QueryHandlers;

public class
    ListAllWeatherForecastQueryHandler : IQueryHandler<ListAllWeatherForecastQuery, IEnumerable<WeatherForecastEntity>>
{
    private readonly IWeatherForecastReadOnlyRepository _readOnlyRespository;

    public ListAllWeatherForecastQueryHandler(IWeatherForecastReadOnlyRepository readOnlyRespository)
    {
        _readOnlyRespository = readOnlyRespository;
    }

    public async Task<IEnumerable<WeatherForecastEntity>> Execute(ListAllWeatherForecastQuery query)
    {
        return await _readOnlyRespository.Select()
            .Include(x => x.Wind)
            .ToListAsync();
    }

    public void Dispose()
    {
        _readOnlyRespository.Dispose();
    }
}