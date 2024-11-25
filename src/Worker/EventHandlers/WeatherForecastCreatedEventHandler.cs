using System;
using System.Threading.Tasks;
using AGTec.Common.Base.Extensions;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.CQRS.EventHandlers;
using Microsoft.Extensions.Logging;
using uServiceDemo.Application.Queries;
using uServiceDemo.Document;
using uServiceDemo.Document.Entities;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Events;

namespace uServiceDemo.Worker.EventHandlers;

internal class WeatherForecastCreatedEventHandler : IEventHandler<WeatherForecastCreatedEvent>
{
    private readonly ILogger<WeatherForecastCreatedEventHandler> _logger;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IWeatherForecastDocRepository _repository;
    private readonly IWeatherForecastSearchRepository _searchRepository;

    public WeatherForecastCreatedEventHandler(IQueryDispatcher queryDispatcher,
        IWeatherForecastDocRepository repository,
        IWeatherForecastSearchRepository searchRepository,
        ILogger<WeatherForecastCreatedEventHandler> logger)
    {
        _queryDispatcher = queryDispatcher;
        _repository = repository;
        _searchRepository = searchRepository;
        _logger = logger;
    }

    public async Task Process(WeatherForecastCreatedEvent evt)
    {
        _logger.LogInformation($"Start processing WeatherForecast created event for ID: {evt.Id}.");

        try
        {
            var query = new GetWeatherForecastByIdQuery(evt.Id);
            var weatherForecastEntity =
                await _queryDispatcher.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(query);

            var weatherForecastDocument = new WeatherForecastDoc(weatherForecastEntity.Id);
            weatherForecastDocument.Date = weatherForecastEntity.Date;
            weatherForecastDocument.Temperature = weatherForecastEntity.Temperature;
            weatherForecastDocument.Summary = weatherForecastEntity.Summary;
            weatherForecastDocument.UpdatedBy = weatherForecastEntity.UpdatedBy;

            if (weatherForecastEntity.Wind != null)
            {
                weatherForecastDocument.WindDirection = weatherForecastEntity.Wind.Direction.GetDescriptionOfEnum();
                weatherForecastDocument.WindSpeed = weatherForecastEntity.Wind.Speed;
            }

            await _repository.Insert(weatherForecastDocument);
            await _searchRepository.Insert(weatherForecastDocument);

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
