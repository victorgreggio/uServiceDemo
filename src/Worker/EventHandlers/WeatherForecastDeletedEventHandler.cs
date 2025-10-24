using System;
using System.Threading.Tasks;
using AGTec.Common.CQRS.EventHandlers;
using Microsoft.Extensions.Logging;
using uServiceDemo.Document;
using uServiceDemo.Events;

namespace uServiceDemo.Worker.EventHandlers;

internal class WeatherForecastDeletedEventHandler : IEventHandler<WeatherForecastDeletedEvent>
{
    private readonly ILogger<WeatherForecastDeletedEventHandler> _logger;
    private readonly IWeatherForecastDocRepository _repository;
    private readonly IWeatherForecastSearchRepository _searchRepository;

    public WeatherForecastDeletedEventHandler(
        IWeatherForecastDocRepository repository,
        IWeatherForecastSearchRepository searchRepository,
        ILogger<WeatherForecastDeletedEventHandler> logger)
    {
        _repository = repository;
        _searchRepository = searchRepository;
        _logger = logger;
    }

    public async Task Process(WeatherForecastDeletedEvent evt)
    {
        _logger.LogInformation($"Start processing WeatherForecast deleted event for ID: {evt.Id}.");

        try
        {
            var document = await _repository.GetById(evt.Id);
            if (document != null)
            {
                await _repository.Delete(document);
                await _searchRepository.Delete(document);
            }

            _logger.LogInformation($"Finished processing WeatherForecast deleted event for ID: {evt.Id}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to process WeatherForecastDeleted event for ID: {evt.Id}");
            throw;
        }
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}
