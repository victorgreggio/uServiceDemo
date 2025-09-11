using System;
using System.Threading.Tasks;
using uServiceDemo.Contracts.Requests;

namespace uServiceDemo.Application.UseCases.AddWeatherForecast.V1;

public interface IAddWeatherForecastUseCase
{
    Task<Guid> Execute(AddWeatherForecastRequest input);
}