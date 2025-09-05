using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using uServiceDemo.Application.UseCases.ListWeatherForecasts.V1;
using uServiceDemo.Contracts;

namespace uServiceDemo.Api;

public static class Endpoints
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/weatherforecast").WithTags("WeatherForecast");

        group.MapGet("/", (IListWeatherForecastsUseCase useCase, ILogger<IListWeatherForecastsUseCase> logger) => TryAndCatch(async () => await useCase.Execute(), logger))
        .Produces<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("GetWeatherForecasts")
        .WithSummary("Get weather forecasts")
        .WithOpenApi();

        return app;
    }

    public static TResult TryAndCatch<TResult, TLogger>(Func<TResult> func, TLogger logger) where TLogger : ILogger
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
