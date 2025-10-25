using System;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.DeleteWeatherForecast.V1;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class DeleteWeatherForecastUseCaseTest_EntityNotFound : AutoMockSpecification<DeleteWeatherForecastUseCase, IDeleteWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private Guid _testId;
    private Exception _exception;

    protected override void GivenThat()
    {
        _testId = Guid.NewGuid();

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

    [TestMethod]
    public void Should_Have_Correct_Error_Message()
    {
        Assert.IsTrue(_exception.Message.Contains(_testId.ToString()));
    }
}
