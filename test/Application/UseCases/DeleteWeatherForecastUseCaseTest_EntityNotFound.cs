using System;
using System.Threading.Tasks;
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
[ExceptionSpecification]
public class DeleteWeatherForecastUseCaseTest_EntityNotFound : AutoMockSpecification<DeleteWeatherForecastUseCase, IDeleteWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private Guid _testId;

    protected override Task GivenThat()
    {
        _testId = Guid.NewGuid();

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(
            It.IsAny<GetWeatherForecastByIdQuery>()))
            .ReturnsAsync((WeatherForecastEntity)null);

        return Task.CompletedTask;
    }

    protected override async Task WhenIRun() => await CreateSut().Execute(_testId);

    [TestMethod]
    public void Should_Throw_NotFoundException() => AssertExceptionThrown<NotFoundException>();
}
