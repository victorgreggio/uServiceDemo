using System;
using System.Threading.Tasks;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
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
[ExceptionSpecification]
public class UpdateWeatherForecastUseCaseTest_EntityNotFound : AutoMockSpecification<UpdateWeatherForecastUseCase, IUpdateWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private Guid _testId;
    private UpdateWeatherForecastRequest _updateRequest;

    protected override Task GivenThat()
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

        return Task.CompletedTask;
    }

    protected override async Task WhenIRun() => await CreateSut().Execute(_testId, _updateRequest);

    [TestMethod]
    public void Should_Throw_NotFoundException() => AssertExceptionThrown<NotFoundException>();

}
