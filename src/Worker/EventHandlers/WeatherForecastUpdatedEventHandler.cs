using System;
using System.Threading.Tasks;
using AGTec.Common.Base.Extensions;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.CQRS.EventHandlers;
using Microsoft.Extensions.Logging;
using uServiceDemo.Application.Queries;
using uServiceDemo.Document;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Events;

namespace uServiceDemo.Worker.EventHandlers;

internal class WeatherForecastUpdatedEventHandler : IEventHandler<WeatherForecastUpdatedEvent>
{
    private readonly ILogger<WeatherForecastUpdatedEventHandler> _logger;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IWeatherForecastDocRepository _repository;
    private readonly IWeatherForecastSearchRepository _searchRepository;

    public WeatherForecastUpdatedEventHandler(IQueryDispatcher queryDispatcher,
        IWeatherForecastDocRepository repository,
        IWeatherForecastSearchRepository searchRepository,
        ILogger<WeatherForecastUpdatedEventHandler> logger)
    {
        _queryDispatcher = queryDispatcher;
        _repository = repository;
        _searchRepository = searchRepository;
        _logger = logger;
    }

    public async Task Process(WeatherForecastUpdatedEvent evt)
    {
        _logger.LogInformation($"Start processing WeatherForecast updated event for ID: {evt.Id}.");

        try
        {
            var query = new GetWeatherForecastByIdQuery(evt.Id);
            var weatherForecastEntity =
                await _queryDispatcher.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(query);
            var weatherForecastDocument = await _repository.GetById(evt.Id);

            weatherForecastDocument.Date = weatherForecastEntity.Date;
            weatherForecastDocument.Temperature = weatherForecastEntity.Temperature;
            weatherForecastDocument.Summary = weatherForecastEntity.Summary;

            if (weatherForecastEntity.Wind != null)
            {
                weatherForecastDocument.WindDirection = weatherForecastEntity.Wind.Direction.GetDescriptionOfEnum();
                weatherForecastDocument.WindSpeed = weatherForecastEntity.Wind.Speed;
            }

            await _repository.Update(weatherForecastDocument);
            await _searchRepository.Update(weatherForecastDocument);

            _logger.LogInformation($"Finished processing WeatherForecast updated event for ID: {evt.Id}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to process WeatherForecastUpdated event for ID: {evt.Id}");
            throw;
        }
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}