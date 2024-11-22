using System;
using AGTec.Common.Domain.Entities;

namespace uServiceDemo.Domain.Entities;

public class WeatherForecastEntity : Entity
{
    public WeatherForecastEntity(Guid id)
        : base(id)
    {
    }

    public DateTime Date { get; set; }

    public int Temperature { get; set; }

    public string Summary { get; set; }

    public WindEntity Wind { get; set; }
}