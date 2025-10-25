using System;
using System.Security.Principal;
using System.Threading;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.UpdateWeatherForecast.V1;
using uServiceDemo.Contracts.Requests;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class UpdateWeatherForecastUseCaseTest_EntityNotFound : AutoMockSpecification<UpdateWeatherForecastUseCase, IUpdateWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private Guid _testId;
    private UpdateWeatherForecastRequest _updateRequest;
    private Exception _exception;

    protected override void GivenThat()
    {
        _testId = Guid.NewGuid();
        _updateRequest = new UpdateWeatherForecastRequest
        {
            Summary = "Updated",
            TemperatureInCelsius = 25,
            Date = DateTime.UtcNow
        };

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
            CreateSut().Execute(_testId, _updateRequest).Wait();
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

    [TestMethod]
    public void Should_Have_Correct_Error_Message()
    {
        Assert.IsTrue(_exception.Message.Contains(_testId.ToString()));
    }
}
