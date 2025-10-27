using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using uServiceDemo.Contracts;
using uServiceDemo.Contracts.Requests;
using uServiceDemo.Document.Entities;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Events;

namespace uServiceDemo.Application.Mappers;

internal static class MapConfig
{
    public static MapperConfiguration GetMapperConfiguration()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AddWeatherForecastRequest, WeatherForecastEntity>()
                .ForMember(dest => dest.Temperature, opts => opts.MapFrom(source => source.TemperatureInCelsius))
                .ForMember(dest => dest.Date, opts => opts.MapFrom(source =>
                    source.Date.Kind == DateTimeKind.Utc ? source.Date : source.Date.ToUniversalTime()))
                .ForMember(dest => dest.Wind, opts => opts.MapFrom(source => 
                    source.WindSpeed.HasValue && source.WindDirection.HasValue 
                        ? new WindEntity(Guid.NewGuid()) 
                        { 
                            Speed = source.WindSpeed.Value, 
                            Direction = (Domain.Enums.WindDirection)source.WindDirection.Value 
                        }
                        : null));
            cfg.CreateMap<WeatherForecastEntity, WeatherForecast>()
                .ForMember(dest => dest.TemperatureInCelsius, opts => opts.MapFrom(source => source.Temperature))
                .ForMember(dest => dest.WindSpeed, opts => opts.MapFrom(source => source.Wind != null ? source.Wind.Speed : (int?)null))
                .ForMember(dest => dest.WindDirection, opts => opts.MapFrom(source => source.Wind != null ? (uServiceDemo.Contracts.WindDirection?)source.Wind.Direction : null));
            cfg.CreateMap<WeatherForecast, WeatherForecastEntity>();
            cfg.CreateMap<WeatherForecastEntity, WeatherForecastCreatedEvent>();
            cfg.CreateMap<WeatherForecastEntity, WeatherForecastUpdatedEvent>();
            cfg.CreateMap<WeatherForecastDoc, WeatherForecast>()
                .ForMember(dest => dest.TemperatureInCelsius, opts => opts.MapFrom(source => source.Temperature))
                .ForMember(dest => dest.WindSpeed, opts => opts.MapFrom(source => source.WindSpeed != 0 ? source.WindSpeed : (int?)null))
                .ForMember(dest => dest.WindDirection, opts => opts.MapFrom(source => 
                    !string.IsNullOrEmpty(source.WindDirection) ? ParseWindDirectionFromDescription(source.WindDirection) : (uServiceDemo.Contracts.WindDirection?)null));
        }, NullLoggerFactory.Instance);
    }

    private static WindDirection? ParseWindDirectionFromDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            return null;

        return description.ToLower() switch
        {
            "north" => WindDirection.N,
            "northeast" => WindDirection.NE,
            "east" => WindDirection.E,
            "southeast" => WindDirection.SE,
            "south" => WindDirection.S,
            "southwest" => WindDirection.SW,
            "west" => WindDirection.W,
            "northwest" => WindDirection.NW,
            _ => null
        };
    }
}