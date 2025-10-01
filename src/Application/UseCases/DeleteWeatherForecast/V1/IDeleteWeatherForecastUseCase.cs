using System;
using System.Threading.Tasks;

namespace uServiceDemo.Application.UseCases.DeleteWeatherForecast.V1;

public interface IDeleteWeatherForecastUseCase
{
    Task Execute(Guid id);
}
