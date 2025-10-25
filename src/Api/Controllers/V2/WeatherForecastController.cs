using Asp.Versioning;
using AGTec.Services.ServiceDefaults.Authentication.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uServiceDemo.Application.UseCases.GetWeatherForecast.V2;
using uServiceDemo.Contracts;

namespace uServiceDemo.Api.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/weatherforecast")]
[ScopeAuthorize("api")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id:guid}", Name = "GetWeatherForecastV2")]
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
}
