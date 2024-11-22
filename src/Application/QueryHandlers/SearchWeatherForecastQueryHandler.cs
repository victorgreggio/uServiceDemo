using System.Collections.Generic;
using System.Threading.Tasks;
using AGTec.Common.CQRS.QueryHandlers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Document;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Application.QueryHandlers;

public class
    SearchWeatherForecastQueryHandler : IQueryHandler<SearchWeatherForecastQuery, IEnumerable<WeatherForecastDoc>>
{
    private readonly IWeatherForecastSearchReadOnlyRepository _readOnlyRespository;

    public SearchWeatherForecastQueryHandler(IWeatherForecastSearchReadOnlyRepository readOnlyRespository)
    {
        _readOnlyRespository = readOnlyRespository;
    }

    public async Task<IEnumerable<WeatherForecastDoc>> Execute(SearchWeatherForecastQuery query)
    {
        return await _readOnlyRespository
            .Search(e => e.Summary, query.Term);
    }

    public void Dispose()
    {
        _readOnlyRespository.Dispose();
    }
}