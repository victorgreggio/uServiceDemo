using AGTec.Common.Monitor;
using AGTec.Services.ServiceDefaults;
using Correlate.AspNetCore;
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

app.UseStaticFiles();
app.UseRouting();
app.UseAGTecMonitor(app.Environment);
app.UseCorrelate();
//app.UseAuthentication();
//app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapOpenApi();

app.MapEndpoints();

app.Run();
