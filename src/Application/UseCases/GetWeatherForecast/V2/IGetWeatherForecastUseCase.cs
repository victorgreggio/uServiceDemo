using System;
using System.Threading.Tasks;
using uServiceDemo.Contracts;

namespace uServiceDemo.Application.UseCases.GetWeatherForecast.V2;

public interface IGetWeatherForecastUseCase
{
    Task<WeatherForecast> Execute(Guid id);
}