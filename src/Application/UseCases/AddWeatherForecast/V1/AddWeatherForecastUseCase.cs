using AGTec.Common.BackgroundTaskQueue;
using AGTec.Common.CQRS.Dispatchers;
using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using uServiceDemo.Application.Commands;
using uServiceDemo.Contracts.Requests;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Events;

namespace uServiceDemo.Application.UseCases.AddWeatherForecast.V1;

public class AddWeatherForecastUseCase : IAddWeatherForecastUseCase
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IMapper _mapper;

    public AddWeatherForecastUseCase(ICommandDispatcher commandDispatcher,
        IEventDispatcher eventDispatcher,
        IBackgroundTaskQueue backgroundTaskQueue,
        IMapper mapper)
    {
        _commandDispatcher = commandDispatcher;
        _eventDispatcher = eventDispatcher;
        _backgroundTaskQueue = backgroundTaskQueue;
        _mapper = mapper;
    }

    public async Task<Guid> Execute(AddWeatherForecastRequest input)
    {
        var weatherForecast = _mapper.Map(input, new WeatherForecastEntity(Guid.NewGuid()));

        var command = new CreateWeatherForecastCommand(weatherForecast, Thread.CurrentPrincipal?.Identity?.Name);
        await _commandDispatcher.Execute(command);

        var evt = _mapper.Map<WeatherForecastEntity, WeatherForecastCreatedEvent>(weatherForecast);

        _backgroundTaskQueue.Queue($"Publishing WeatherForecastCreatedEvent for {evt.Id}",
            cancelationToken => _eventDispatcher.Raise(evt));

        return weatherForecast.Id;
    }
}