using System;
using System.Collections.Generic;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.SearchWeatherForecast.V1;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class SearchWeatherForecastUseCaseTest_EmptyResults : AutoMockSpecification<SearchWeatherForecastUseCase, ISearchWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private string _searchTerm;
    private Exception _exception;

    protected override void GivenThat()
    {
        _searchTerm = "nonexistent";

        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<SearchWeatherForecastQuery, IEnumerable<WeatherForecastDoc>>(
            It.IsAny<SearchWeatherForecastQuery>()))
            .ReturnsAsync(new List<WeatherForecastDoc>());
    }

    protected override void WhenIRun()
    {
        try
        {
            CreateSut().Execute(_searchTerm).Wait();
        }
        catch (Exception ex)
        {
            _exception = ex.InnerException ?? ex;
        }
    }

    [TestMethod]
    public void Should_Throw_NotFoundException()
    {
        Assert.IsInstanceOfType(_exception, typeof(NotFoundException));
    }
}
