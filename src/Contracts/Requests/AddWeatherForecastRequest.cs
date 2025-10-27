using System;
using System.ComponentModel.DataAnnotations;
using uServiceDemo.Contracts;

namespace uServiceDemo.Contracts.Requests;

public class AddWeatherForecastRequest
{
    [Required] public DateTime Date { get; set; }

    [Required] public int TemperatureInCelsius { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(4000)]
    public string Summary { get; set; }

    public int? WindSpeed { get; set; }

    public WindDirection? WindDirection { get; set; }
}