using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.UseCases.GetWeatherForecast.V2;
using uServiceDemo.Application.UseCases.SearchWeatherForecast.V1;
using uServiceDemo.Contracts;

namespace uServiceDemo.Api.Controllers.V2;

[ApiController]
[ApiVersion(Version)]
[Route("api/{version:apiVersion}/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private const string Version = "2.0";
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IServiceProvider _serviceProvider;

    public WeatherForecastController(IServiceProvider serviceProvider,
        ILogger<WeatherForecastController> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
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