using AGTec.Common.BackgroundTaskQueue;
using AGTec.Common.CQRS.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uServiceDemo.Application;
using uServiceDemo.Document;
using uServiceDemo.Events;
using uServiceDemo.Worker.BackgroundServices;
using uServiceDemo.Worker.EventHandlers;

var builder = Host.CreateApplicationBuilder(args);

builder.AddElasticsearchClient(connectionName: "Elasticsearch");
builder.AddMongoDBClient(connectionName: "MongoWeatherforecastDocumentDB");

builder.Services.AddDocumentModule();
builder.Services.AddApplicationModule(builder.Configuration);
builder.Services.AddTransient<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddTransient<IEventHandler<WeatherForecastCreatedEvent>, WeatherForecastCreatedEventHandler>();
builder.Services.AddTransient<IEventHandler<WeatherForecastUpdatedEvent>, WeatherForecastUpdatedEventHandler>();
builder.Services.AddTransient<IEventHandler<WeatherForecastDeletedEvent>, WeatherForecastDeletedEventHandler>();
builder.Services.AddHostedService<WeatherTopicListenerBackgroundService>();

builder.Build().Run();