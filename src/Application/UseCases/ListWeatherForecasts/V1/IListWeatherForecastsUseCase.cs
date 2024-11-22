using System.Collections.Generic;
using System.Threading.Tasks;
using uServiceDemo.Contracts;

namespace uServiceDemo.Application.UseCases.ListWeatherForecasts.V1;

public interface IListWeatherForecastsUseCase
{
    Task<IEnumerable<WeatherForecast>> Execute();
}