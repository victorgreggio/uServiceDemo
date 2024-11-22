using AGTec.Common.CQRS.Commands;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Commands;

internal class CreateWeatherForecastCommand : ICommand
{
    public CreateWeatherForecastCommand(WeatherForecastEntity weatherForecast, string username)
    {
        WeatherForecast = weatherForecast;
        Username = username;
    }

    public WeatherForecastEntity WeatherForecast { get; }
    public string Username { get; }
}