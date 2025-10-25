using AGTec.Common.BackgroundTaskQueue;
using AGTec.Common.CQRS.Dispatchers;
using System;
using System.Threading;
using System.Threading.Tasks;
using uServiceDemo.Application.Commands;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Queries;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Events;

namespace uServiceDemo.Application.UseCases.DeleteWeatherForecast.V1;

public class DeleteWeatherForecastUseCase : IDeleteWeatherForecastUseCase
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public DeleteWeatherForecastUseCase(IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher,
        IEventDispatcher eventDispatcher,
        IBackgroundTaskQueue backgroundTaskQueue)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _eventDispatcher = eventDispatcher;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    public async Task Execute(Guid id)
    {
        var query = new GetWeatherForecastByIdQuery(id);
        var entity = await _queryDispatcher.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(query);

        if (entity == null)
            throw new NotFoundException($"No weather forecast found with ID = '{id}'.");

        var command = new DeleteWeatherForecastCommand(id, Thread.CurrentPrincipal?.Identity?.Name);
        await _commandDispatcher.Execute(command);

        var evt = new WeatherForecastDeletedEvent { Id = id };

        _backgroundTaskQueue.Queue($"Publishing WeatherForecastDeletedEvent for {evt.Id}",
            cancelationToken => _eventDispatcher.Raise(evt));
    }
}
