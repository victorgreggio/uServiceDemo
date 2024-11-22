using System.Threading;
using System.Threading.Tasks;
using AGTec.Common.BackgroundTaskQueue;
using AGTec.Common.CQRS.Messaging;
using Microsoft.Extensions.Logging;

namespace uServiceDemo.Worker.BackgroundServices;

internal class WeatherTopicListenerBackgroundService : BackgroundService<int>
{
    private const string TOPIC_NAME = "weather";

    private readonly IMessageHandler _messageHandler;
    private ILogger<WeatherTopicListenerBackgroundService> _logger;

    public WeatherTopicListenerBackgroundService(IMessageHandler messageHandler,
        ILogger<WeatherTopicListenerBackgroundService> logger)
    {
        _messageHandler = messageHandler;
        _logger = logger;
    }

    protected override Task<int> ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageHandler.Handle(TOPIC_NAME, PublishType.Topic, GetType().Name);
        _logger.LogInformation($"Start listening for messages on topic: {TOPIC_NAME}");

        while (stoppingToken.IsCancellationRequested == false) Thread.Sleep(100);

        return Task.FromResult(0);
    }
}