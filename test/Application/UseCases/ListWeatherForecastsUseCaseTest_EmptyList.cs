using System.Collections.Generic;
using System.Linq;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.ListWeatherForecasts.V1;
using uServiceDemo.Contracts;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class ListWeatherForecastsUseCaseTest_EmptyList : AutoMockSpecification<ListWeatherForecastsUseCase, IListWeatherForecastsUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private IEnumerable<WeatherForecast> _result;

    protected override void GivenThat()
    {
        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<ListAllWeatherForecastQuery, IEnumerable<WeatherForecastEntity>>(
            It.IsAny<ListAllWeatherForecastQuery>()))
            .ReturnsAsync(new List<WeatherForecastEntity>());
    }

    protected override void WhenIRun()
    {
        _result = CreateSut().Execute().Result;
    }

    [TestMethod]
    public void Should_Return_Empty_List()
    {
        Assert.IsNotNull(_result);
        Assert.AreEqual(0, _result.Count());
    }
}
