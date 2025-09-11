using AGTec.Common.BackgroundTaskQueue;
using AGTec.Common.CQRS.Dispatchers;
using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using uServiceDemo.Application.Commands;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Queries;
using uServiceDemo.Contracts.Requests;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Events;

namespace uServiceDemo.Application.UseCases.UpdateWeatherForecast.V1;

internal class UpdateWeatherForecastUseCase : IUpdateWeatherForecastUseCase
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IMapper _mapper;
    private readonly IQueryDispatcher _queryDispatcher;

    public UpdateWeatherForecastUseCase(IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher,
        IEventDispatcher eventDispatcher,
        IBackgroundTaskQueue backgroundTaskQueue,
        IMapper mapper)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _eventDispatcher = eventDispatcher;
        _backgroundTaskQueue = backgroundTaskQueue;
        _mapper = mapper;
    }

    public async Task Execute(Guid id, UpdateWeatherForecastRequest input)
    {
        var query = new GetWeatherForecastByIdQuery(id);
        var entity = await _queryDispatcher.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(query);

        if (entity == null)
            throw new NotFoundException($"No weather forecast found with ID = '{id}'.");

        entity.Temperature = input.TemperatureInCelsius;
        entity.Date = input.Date;
        entity.Summary = input.Summary;

        var command = new UpdateWeatherForecastCommand(entity, Thread.CurrentPrincipal?.Identity?.Name);
        await _commandDispatcher.Execute(command);

        var evt = _mapper.Map<WeatherForecastEntity, WeatherForecastUpdatedEvent>(entity);

        _backgroundTaskQueue.Queue($"Publishing WeatherForecastUpdatedEvent for {evt.Id}",
            cancelationToken => _eventDispatcher.Raise(evt));
    }
}