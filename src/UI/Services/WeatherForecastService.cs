using System.Net.Http.Json;
using uServiceDemo.Contracts;
using uServiceDemo.Contracts.Requests;

namespace uServiceDemo.UI.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "/api/weatherforecast";

    public WeatherForecastService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WeatherForecast>> GetAllAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(BaseUrl);
        return response ?? Array.Empty<WeatherForecast>();
    }

    public async Task<WeatherForecast?> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<WeatherForecast>($"{BaseUrl}/{id}");
    }

    public async Task<Guid> CreateAsync(AddWeatherForecastRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, request);
        response.EnsureSuccessStatusCode();
        var id = await response.Content.ReadFromJsonAsync<Guid>();
        return id;
    }

    public async Task UpdateAsync(Guid id, UpdateWeatherForecastRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
