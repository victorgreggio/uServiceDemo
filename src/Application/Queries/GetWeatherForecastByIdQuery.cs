using System;
using AGTec.Common.CQRS.Queries;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Queries;

public class GetWeatherForecastByIdQuery : IQuery<WeatherForecastEntity>
{
    public GetWeatherForecastByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}