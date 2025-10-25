using System;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.GetWeatherForecast.V1;
using uServiceDemo.Contracts;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class GetWeatherForecastUseCaseTest_EntityNotFound : AutoMockSpecification<GetWeatherForecastUseCase, IGetWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private Guid _testId;
    private Exception _exception;

    protected override void GivenThat()
    {
        _testId = Guid.NewGuid();

        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(
            It.IsAny<GetWeatherForecastByIdQuery>()))
            .ReturnsAsync((WeatherForecastEntity)null);
    }

    protected override void WhenIRun()
    {
        try
        {
            CreateSut().Execute(_testId).Wait();
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
