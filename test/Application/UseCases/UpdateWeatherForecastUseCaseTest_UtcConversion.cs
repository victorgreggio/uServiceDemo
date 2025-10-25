using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Commands;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.UpdateWeatherForecast.V1;
using uServiceDemo.Contracts.Requests;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class UpdateWeatherForecastUseCaseTest_UtcConversion : AutoMockSpecification<UpdateWeatherForecastUseCase, IUpdateWeatherForecastUseCase>
{
    private Mock<ICommandDispatcher> _commandDispatcher;
    private Mock<IQueryDispatcher> _queryDispatcher;
    private Guid _testId;
    private WeatherForecastEntity _existingEntity;
    private UpdateWeatherForecastRequest _updateRequest;

    protected override void GivenThat()
    {
        _testId = Guid.NewGuid();
        _existingEntity = new WeatherForecastEntity(_testId);

        var localDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local);
        _updateRequest = new UpdateWeatherForecastRequest
        {
            Summary = "Test",
            TemperatureInCelsius = 20,
            Date = localDate
        };

        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(
            It.IsAny<GetWeatherForecastByIdQuery>()))
            .ReturnsAsync(_existingEntity);

        _commandDispatcher = Dep<ICommandDispatcher>();
        _commandDispatcher.Setup(x => x.Execute(It.IsAny<UpdateWeatherForecastCommand>()))
            .Returns(Task.CompletedTask);
    }

    protected override void WhenIRun()
    {
        CreateSut().Execute(_testId, _updateRequest).Wait();
    }

    [TestMethod]
    public void Should_Convert_Date_To_Utc()
    {
        _commandDispatcher.Verify(x => x.Execute(It.Is<UpdateWeatherForecastCommand>(cmd =>
            cmd.WeatherForecast.Date.Kind == DateTimeKind.Utc
        )), Times.Once);
    }
}
