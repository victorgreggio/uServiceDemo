using System.Threading.Tasks;
using AGTec.Common.BackgroundTaskQueue;
using AGTec.Common.CQRS.EventHandlers;
using AGTec.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uServiceDemo.Application;
using uServiceDemo.Document;
using uServiceDemo.Events;
using uServiceDemo.Worker.BackgroundServices;
using uServiceDemo.Worker.EventHandlers;

namespace uServiceDemo.Worker;

public class WeatherForecastWorker
{
    public static async Task Main(string[] args)
    {
        var host = HostBuilderFactory.CreateHostBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDocumentModule(hostContext.Configuration);
                services.AddApplicationModule(hostContext.Configuration);
                services.AddTransient<IBackgroundTaskQueue, BackgroundTaskQueue>();
                services.AddTransient<IEventHandler<WeatherForecastCreatedEvent>, WeatherForecastCreatedEventHandler>();
                services.AddTransient<IEventHandler<WeatherForecastUpdatedEvent>, WeatherForecastUpdatedEventHandler>();
                services.AddHostedService<WeatherTopicListenerBackgroundService>();
            })
            .Build();

        await host.RunAsync();
    }
}