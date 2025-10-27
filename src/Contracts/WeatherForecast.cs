using System;
using uServiceDemo.Contracts;

namespace uServiceDemo.Contracts;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureInCelsius { get; set; }

    public int TemperatureInFahrenheit => 32 + (int)(TemperatureInCelsius / 0.5556);

    public string Summary { get; set; }

    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastUpdated { get; set; }

    public string UpdatedBy { get; set; }

    public int? WindSpeed { get; set; }

    public WindDirection? WindDirection { get; set; }
}