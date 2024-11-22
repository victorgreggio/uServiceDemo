using System;
using System.ComponentModel.DataAnnotations;

namespace uServiceDemo.Contracts.Requests;

public class UpdateWeatherForecastRequest
{
    [Required] public DateTime Date { get; set; }

    [Required] public int TemperatureInCelsius { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(4000)]
    public string Summary { get; set; }
}