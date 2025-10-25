using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Contracts;
using uServiceDemo.Contracts.Requests;
using uServiceDemo.Document.Entities;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Events;

namespace uServiceDemo.Application.Test.Mappers;

[TestClass]
public class MapConfigTest
{
    [TestMethod]
    public void Should_Have_Valid_Mapper_Configuration()
    {
        var config = MapConfig.GetMapperConfiguration();
        var mapper = config.CreateMapper();
        
        Assert.IsNotNull(mapper);
    }

    [TestMethod]
    public void Should_Map_AddWeatherForecastRequest_To_Entity()
    {
        var mapper = MapConfig.GetMapperConfiguration().CreateMapper();
        
        var request = new AddWeatherForecastRequest
        {
            Summary = "Sunny",
            TemperatureInCelsius = 25,
            Date = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local)
        };

        var entity = new WeatherForecastEntity(Guid.NewGuid());
        var result = mapper.Map(request, entity);

        Assert.AreEqual(request.Summary, result.Summary);
        Assert.AreEqual(request.TemperatureInCelsius, result.Temperature);
        Assert.AreEqual(DateTimeKind.Utc, result.Date.Kind);
    }

    [TestMethod]
    public void Should_Map_AddWeatherForecastRequest_With_Utc_Date()
    {
        var mapper = MapConfig.GetMapperConfiguration().CreateMapper();
        
        var utcDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var request = new AddWeatherForecastRequest
        {
            Summary = "Sunny",
            TemperatureInCelsius = 25,
            Date = utcDate
        };

        var entity = new WeatherForecastEntity(Guid.NewGuid());
        var result = mapper.Map(request, entity);

        Assert.AreEqual(utcDate, result.Date);
        Assert.AreEqual(DateTimeKind.Utc, result.Date.Kind);
    }

    [TestMethod]
    public void Should_Map_WeatherForecastEntity_To_Contract()
    {
        var mapper = MapConfig.GetMapperConfiguration().CreateMapper();
        
        var entity = new WeatherForecastEntity(Guid.NewGuid())
        {
            Summary = "Cloudy",
            Temperature = 18,
            Date = DateTime.UtcNow
        };

        var result = mapper.Map<WeatherForecastEntity, WeatherForecast>(entity);

        Assert.AreEqual(entity.Id, result.Id);
        Assert.AreEqual(entity.Summary, result.Summary);
        Assert.AreEqual(entity.Temperature, result.TemperatureInCelsius);
        Assert.AreEqual(entity.Date, result.Date);
    }

    [TestMethod]
    public void Should_Map_WeatherForecast_To_Entity()
    {
        var mapper = MapConfig.GetMapperConfiguration().CreateMapper();
        
        var contract = new WeatherForecast
        {
            Id = Guid.NewGuid(),
            Summary = "Rainy",
            TemperatureInCelsius = 15,
            Date = DateTime.UtcNow
        };

        var result = mapper.Map<WeatherForecast, WeatherForecastEntity>(contract);

        Assert.AreEqual(contract.Id, result.Id);
        Assert.AreEqual(contract.Summary, result.Summary);
        Assert.AreEqual(contract.Date, result.Date);
    }

    [TestMethod]
    public void Should_Map_WeatherForecastEntity_To_CreatedEvent()
    {
        var mapper = MapConfig.GetMapperConfiguration().CreateMapper();
        
        var entity = new WeatherForecastEntity(Guid.NewGuid())
        {
            Summary = "Stormy",
            Temperature = 12,
            Date = DateTime.UtcNow
        };

        var result = mapper.Map<WeatherForecastEntity, WeatherForecastCreatedEvent>(entity);

        Assert.AreEqual(entity.Id, result.Id);
    }

    [TestMethod]
    public void Should_Map_WeatherForecastEntity_To_UpdatedEvent()
    {
        var mapper = MapConfig.GetMapperConfiguration().CreateMapper();
        
        var entity = new WeatherForecastEntity(Guid.NewGuid())
        {
            Summary = "Windy",
            Temperature = 20,
            Date = DateTime.UtcNow
        };

        var result = mapper.Map<WeatherForecastEntity, WeatherForecastUpdatedEvent>(entity);

        Assert.AreEqual(entity.Id, result.Id);
    }

    [TestMethod]
    public void Should_Map_WeatherForecastDoc_To_Contract()
    {
        var mapper = MapConfig.GetMapperConfiguration().CreateMapper();
        
        var docId = Guid.NewGuid();
        var doc = new WeatherForecastDoc(docId)
        {
            Summary = "Foggy",
            Temperature = 10,
            Date = DateTime.UtcNow
        };

        var result = mapper.Map<WeatherForecastDoc, WeatherForecast>(doc);

        Assert.AreEqual(doc.Id, result.Id);
        Assert.AreEqual(doc.Summary, result.Summary);
        Assert.AreEqual(doc.Temperature, result.TemperatureInCelsius);
        Assert.AreEqual(doc.Date, result.Date);
    }
}
