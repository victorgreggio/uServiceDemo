using uServiceDemo.Contracts;
using uServiceDemo.Contracts.Requests;

namespace uServiceDemo.UI.Services;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
    Task<WeatherForecast?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(AddWeatherForecastRequest request);
    Task UpdateAsync(Guid id, UpdateWeatherForecastRequest request);
    Task DeleteAsync(Guid id);
}
