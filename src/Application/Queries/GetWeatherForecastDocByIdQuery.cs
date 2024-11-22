using System;
using AGTec.Common.CQRS.Queries;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Application.Queries;

internal class GetWeatherForecastDocByIdQuery : IQuery<WeatherForecastDoc>
{
    public GetWeatherForecastDocByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}