using System.Collections.Generic;
using AGTec.Common.CQRS.Queries;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Application.Queries;

public class SearchWeatherForecastQuery : IQuery<IEnumerable<WeatherForecastDoc>>
{
    public SearchWeatherForecastQuery(string term)
    {
        Term = term;
    }

    public string Term { get; }
}