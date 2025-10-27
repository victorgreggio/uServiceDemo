using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.ListWeatherForecasts.V1;
using uServiceDemo.Contracts;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class ListWeatherForecastsUseCaseTest : AutoMockSpecification<ListWeatherForecastsUseCase, IListWeatherForecastsUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private IEnumerable<WeatherForecastEntity> _entities;
    private IEnumerable<WeatherForecast> _result;

    protected override Task GivenThat()
    {
        _entities =
        [
            new(Guid.NewGuid()) { Summary = "Sunny", Temperature = 25, Date = DateTime.UtcNow },
            new(Guid.NewGuid()) { Summary = "Rainy", Temperature = 15, Date = DateTime.UtcNow },
            new(Guid.NewGuid()) { Summary = "Windy", Temperature = 20, Date = DateTime.UtcNow }
        ];

        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<ListAllWeatherForecastQuery, IEnumerable<WeatherForecastEntity>>(
            It.IsAny<ListAllWeatherForecastQuery>()))
            .ReturnsAsync(_entities);

        return Task.CompletedTask;
    }

    protected override async Task WhenIRun() => _result = await CreateSut().Execute();

    [TestMethod]
    public void Should_Query_For_All_Entities()
    {
        _queryDispatcher.Verify(x => x.Execute<ListAllWeatherForecastQuery, IEnumerable<WeatherForecastEntity>>(
            It.IsAny<ListAllWeatherForecastQuery>()), Times.Once);
    }

    [TestMethod]
    public void Should_Return_Correct_Count()
    {
        Assert.AreEqual(_entities.Count(), _result.Count());
    }

    [TestMethod]
    public void Should_Map_All_Entities()
    {
        Assert.AreEqual(3, _result.Count());
        var resultList = _result.ToList();
        var entityList = _entities.ToList();
        
        for (int i = 0; i < entityList.Count; i++)
        {
            Assert.AreEqual(entityList[i].Id, resultList[i].Id);
            Assert.AreEqual(entityList[i].Summary, resultList[i].Summary);
            Assert.AreEqual(entityList[i].Temperature, resultList[i].TemperatureInCelsius);
        }
    }
}
