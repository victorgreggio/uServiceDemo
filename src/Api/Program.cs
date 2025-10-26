using AGTec.Services.ServiceDefaults;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uServiceDemo.Application;
using uServiceDemo.Infrastructure.Repositories.Context;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddRabbitMQClient(connectionName: "RabbitMQ");
        builder.AddElasticsearchClient(connectionName: "Elasticsearch");
        builder.AddMongoDBClient(connectionName: "MongoWeatherforecastDocumentDB");

        builder.AddServiceDefaults<WeatherForecastDbContext>();
        builder.Services.AddApplicationModule(builder.Configuration);

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseServiceDefaults<WeatherForecastDbContext>();

        app.MapControllers();

        app.Run();
    }
}