using Asp.Versioning;
using AGTec.Services.ServiceDefaults.Authentication.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uServiceDemo.Application.UseCases.AddWeatherForecast.V1;
using uServiceDemo.Application.UseCases.DeleteWeatherForecast.V1;
using uServiceDemo.Application.UseCases.GetWeatherForecast.V1;
using uServiceDemo.Application.UseCases.ListWeatherForecasts.V1;
using uServiceDemo.Application.UseCases.SearchWeatherForecast.V1;
using uServiceDemo.Application.UseCases.UpdateWeatherForecast.V1;
using uServiceDemo.Contracts;
using uServiceDemo.Contracts.Requests;

namespace uServiceDemo.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/weatherforecast")]
[ScopeAuthorize("api")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "ListWeatherForecastsV1")]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListWeatherForecasts([FromServices] IListWeatherForecastsUseCase useCase)
    {
        try
        {
            var result = await useCase.Execute();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("search", Name = "SearchWeatherForecastsV1")]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchWeatherForecasts([FromQuery] string term, [FromServices] ISearchWeatherForecastUseCase useCase)
    {
        try
        {
            var result = await useCase.Execute(term);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("{id:guid}", Name = "GetWeatherForecastV1")]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecast(Guid id, [FromServices] IGetWeatherForecastUseCase useCase)
    {
        try
        {
            var result = await useCase.Execute(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpPost(Name = "AddWeatherForecastV1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddWeatherForecast([FromBody] AddWeatherForecastRequest request, [FromServices] IAddWeatherForecastUseCase useCase)
    {
        try
        {
            var result = await useCase.Execute(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpPost("{id:guid}", Name = "UpdateWeatherForecastV1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWeatherForecast(Guid id, [FromBody] UpdateWeatherForecastRequest request, [FromServices] IUpdateWeatherForecastUseCase useCase)
    {
        try
        {
            await useCase.Execute(id, request);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpDelete("{id:guid}", Name = "DeleteWeatherForecastV1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteWeatherForecast(Guid id, [FromServices] IDeleteWeatherForecastUseCase useCase)
    {
        try
        {
            await useCase.Execute(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
