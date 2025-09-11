using System;
using System.Threading.Tasks;
using uServiceDemo.Contracts.Requests;

namespace uServiceDemo.Application.UseCases.UpdateWeatherForecast.V1;

public interface IUpdateWeatherForecastUseCase
{
    Task Execute(Guid id, UpdateWeatherForecastRequest input);
}