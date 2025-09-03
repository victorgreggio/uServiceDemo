using AGTec.Microservice;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using uServiceDemo.Application;
using uServiceDemo.Infrastructure.Repositories.Context;

var builder = WebApplication.CreateBuilder(args);
builder.AddElasticsearchClient(connectionName: "Elasticsearch");
builder.AddMongoDBClient(connectionName: "MongoWeatherforecastDocumentDB");
builder.Services.AddAGTecMicroservice<WeatherForecastDbContext>(builder.Configuration, builder.Environment);
builder.Services.AddApplicationModule(builder.Configuration);

var app = builder.Build();

app.UseAGTecMicroservice<WeatherForecastDbContext>(builder.Environment);

app.Run();
