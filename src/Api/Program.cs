using AGTec.Services.ServiceDefaults;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uServiceDemo.Api;
using uServiceDemo.Application;
using uServiceDemo.Infrastructure.Repositories.Context;

var builder = WebApplication.CreateBuilder(args);

builder.AddElasticsearchClient(connectionName: "Elasticsearch");
builder.AddMongoDBClient(connectionName: "MongoWeatherforecastDocumentDB");

builder.AddServiceDefaults<WeatherForecastDbContext>();
builder.Services.AddApplicationModule(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.UseServiceDefaults<WeatherForecastDbContext>();
app.MapEndpoints();

app.Run();
