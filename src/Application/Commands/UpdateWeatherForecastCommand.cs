using AGTec.Common.CQRS.Commands;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Commands;

internal class UpdateWeatherForecastCommand : ICommand
{
    public UpdateWeatherForecastCommand(WeatherForecastEntity weatherForecast, string username)
    {
        WeatherForecast = weatherForecast;
        Username = username;
    }

    public WeatherForecastEntity WeatherForecast { get; }
    public string Username { get; }
}