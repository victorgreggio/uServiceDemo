using System.Collections.Generic;
using System.Threading.Tasks;
using uServiceDemo.Contracts;

namespace uServiceDemo.Application.UseCases.SearchWeatherForecast.V1;

public interface ISearchWeatherForecastUseCase
{
    Task<IEnumerable<WeatherForecast>> Execute(string term);
}