using AGTec.Services.ServiceDefaults;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using uServiceDemo.Api;
using uServiceDemo.Application;
using uServiceDemo.Infrastructure.Repositories.Context;

var builder = WebApplication.CreateBuilder(args);

builder.AddElasticsearchClient(connectionName: "Elasticsearch");
builder.AddMongoDBClient(connectionName: "MongoWeatherforecastDocumentDB");

builder.AddServiceDefaults<WeatherForecastDbContext>();
builder.Services.AddApplicationModule(builder.Configuration);

var app = builder.Build();

app.UseServiceDefaults<WeatherForecastDbContext>();
app.MapEndpoints();

app.Run();
