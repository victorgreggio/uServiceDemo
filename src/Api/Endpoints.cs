using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using uServiceDemo.Application.UseCases.AddWeatherForecast.V1;
using uServiceDemo.Application.UseCases.DeleteWeatherForecast.V1;
using uServiceDemo.Application.UseCases.GetWeatherForecast.V2;
using uServiceDemo.Application.UseCases.ListWeatherForecasts.V1;
using uServiceDemo.Application.UseCases.UpdateWeatherForecast.V1;
using uServiceDemo.Contracts;
using uServiceDemo.Contracts.Requests;

namespace uServiceDemo.Api;

public static class Endpoints
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/weatherforecast").WithTags("WeatherForecast");

        group.MapGet("/", (IListWeatherForecastsUseCase useCase, ILogger<IListWeatherForecastsUseCase> logger) =>
            TryAndCatch(async () => await useCase.Execute(), logger))
        .Produces<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("ListWeatherForecasts")
        .WithSummary("List weather forecasts")
        .WithOpenApi();

        group.MapGet("/{id:guid}", (Guid id, IGetWeatherForecastUseCase useCase, ILogger<IGetWeatherForecastUseCase> logger) =>
           TryAndCatch(async () => await useCase.Execute(id), logger))
       .Produces<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)
       .Produces(StatusCodes.Status500InternalServerError)
       .WithName("GetWeatherForecasts")
       .WithSummary("Gets a weather forecasts")
       .WithOpenApi();

        group.MapPost("/", (AddWeatherForecastRequest request, IAddWeatherForecastUseCase useCase, ILogger<IAddWeatherForecastUseCase> logger) =>
            TryAndCatch(async () => await useCase.Execute(request), logger))
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("AddWeatherForecast")
        .WithSummary("Add a new weather forecast")
        .WithOpenApi();

        group.MapPost("/{id:guid}", (Guid id, UpdateWeatherForecastRequest request, IUpdateWeatherForecastUseCase useCase, ILogger<IUpdateWeatherForecastUseCase> logger) =>
            TryAndCatch(async () => await useCase.Execute(id, request), logger))
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("UpdateWeatherForecast")
        .WithSummary("Updates an existing weather forecast")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", (Guid id, IDeleteWeatherForecastUseCase useCase, ILogger<IDeleteWeatherForecastUseCase> logger) =>
            TryAndCatch(async () => await useCase.Execute(id), logger))
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithName("DeleteWeatherForecast")
        .WithSummary("Deletes a weather forecast")
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
