using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGTec.Common.CQRS.Dispatchers;
using AutoMapper;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Queries;
using uServiceDemo.Contracts;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Application.UseCases.SearchWeatherForecast.V1;

public class SearchWeatherForecastUseCase : ISearchWeatherForecastUseCase
{
    private readonly IMapper _mapper;
    private readonly IQueryDispatcher _queryDispatcher;

    public SearchWeatherForecastUseCase(IQueryDispatcher queryDispatcher,
        IMapper mapper)
    {
        _queryDispatcher = queryDispatcher;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WeatherForecast>> Execute(string term)
    {
        var query = new SearchWeatherForecastQuery(term);
        var documents =
            await _queryDispatcher.Execute<SearchWeatherForecastQuery, IEnumerable<WeatherForecastDoc>>(query);

        if (documents == null || documents.Any() == false)
            throw new NotFoundException($"No weather forecast found document with term '{term}' on its summary.");

        return _mapper.Map<IEnumerable<WeatherForecastDoc>, IEnumerable<WeatherForecast>>(documents);
    }
}