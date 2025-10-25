using System.Collections.Generic;
using AGTec.Common.CQRS.Queries;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Queries;

public class ListAllWeatherForecastQuery : IQuery<IEnumerable<WeatherForecastEntity>>
{
}