using System.Collections.Generic;
using System.Threading.Tasks;
using AGTec.Common.CQRS.QueryHandlers;
using Microsoft.EntityFrameworkCore;
using uServiceDemo.Application.Queries;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Infrastructure.Repositories;

namespace uServiceDemo.Application.QueryHandlers;

internal class
    ListAllWeatherForecastQueryHandler : IQueryHandler<ListAllWeatherForecastQuery, IEnumerable<WeatherForecastEntity>>
{
    private readonly IWeatherForecastReadOnlyRepository _readOnlyRespository;

    public ListAllWeatherForecastQueryHandler(IWeatherForecastReadOnlyRepository readOnlyRespository)
    {
        _readOnlyRespository = readOnlyRespository;
    }

    public async Task<IEnumerable<WeatherForecastEntity>> Execute(ListAllWeatherForecastQuery query)
    {
        return await _readOnlyRespository.Select().ToListAsync();
    }

    public void Dispose()
    {
        _readOnlyRespository.Dispose();
    }
}