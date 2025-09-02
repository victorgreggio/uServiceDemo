using AGTec.Common.Base.Accessors;
using AGTec.Common.Base.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.UseCases.AddWeatherForecast.V1;
using uServiceDemo.Application.UseCases.GetWeatherForecast.V1;
using uServiceDemo.Application.UseCases.ListWeatherForecasts.V1;
using uServiceDemo.Application.UseCases.SearchWeatherForecast.V1;
using uServiceDemo.Application.UseCases.UpdateWeatherForecast.V1;
using uServiceDemo.Contracts;
using uServiceDemo.Contracts.Requests;

namespace uServiceDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IServiceProvider _serviceProvider;

    public WeatherForecastController(IServiceProvider serviceProvider,
        ILogger<WeatherForecastController> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WeatherForecast>))]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> List()
    {
        using (MiniProfiler.Current.Step("Listing all WeatherForecast"))
        {
            try
            {
                var useCase = _serviceProvider.GetService<IListWeatherForecastsUseCase>();
                var result = await useCase.Execute();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Add([FromBody] AddWeatherForecastRequest input)
    {
        try
        {
            var useCase = _serviceProvider.GetService<IAddWeatherForecastUseCase>();
            var result = await useCase.Execute(input, PrincipalAccessor.Principal.GetUsernameFromClaim());
            return CreatedAtAction(nameof(Get), new { id = result }, input);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(WeatherForecast))]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        try
        {
            var useCase = _serviceProvider.GetService<IGetWeatherForecastUseCase>();
            var result = await useCase.Execute(id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWeatherForecastRequest input)
    {
        try
        {
            var useCase = _serviceProvider.GetService<IUpdateWeatherForecastUseCase>();
            await useCase.Execute(id, input, PrincipalAccessor.Principal.GetUsernameFromClaim());
            return Ok();
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<WeatherForecast>))]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Search([FromQuery] string term)
    {
        try
        {
            var useCase = _serviceProvider.GetService<ISearchWeatherForecastUseCase>();
            var result = await useCase.Execute(term);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}