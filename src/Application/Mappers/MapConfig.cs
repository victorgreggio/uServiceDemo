using AutoMapper;
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
            cfg.CreateMap<AddWeatherForecastRequest, WeatherForecastEntity>().ForMember(
                dest => dest.Temperature, opts => opts.MapFrom(source => source.TemperatureInCelsius));
            cfg.CreateMap<WeatherForecastEntity, WeatherForecast>()
                .ForMember(dest => dest.TemperatureInCelsius, opts => opts.MapFrom(source => source.Temperature));
            cfg.CreateMap<WeatherForecast, WeatherForecastEntity>();
            cfg.CreateMap<WeatherForecastEntity, WeatherForecastCreatedEvent>();
            cfg.CreateMap<WeatherForecastEntity, WeatherForecastUpdatedEvent>();
            cfg.CreateMap<WeatherForecastDoc, WeatherForecast>()
                .ForMember(dest => dest.TemperatureInCelsius, opts => opts.MapFrom(source => source.Temperature));
        });
    }
}