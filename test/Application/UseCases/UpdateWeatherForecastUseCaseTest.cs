using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AGTec.Common.BackgroundTaskQueue;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Randomizer.Impl;
using AGTec.Common.Test;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Commands;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.UpdateWeatherForecast.V1;
using uServiceDemo.Contracts.Requests;
using uServiceDemo.Domain.Entities;
using uServiceDemo.Events;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class UpdateWeatherForecastUseCaseTest : AutoMockSpecification<UpdateWeatherForecastUseCase, IUpdateWeatherForecastUseCase>
{
    private Mock<IBackgroundTaskQueue> _backgroundTaskQueue;
    private Mock<ICommandDispatcher> _commandDispatcher;
    private Mock<IEventDispatcher> _eventDispatcher;
    private Mock<IQueryDispatcher> _queryDispatcher;
    private Guid _testId;
    private WeatherForecastEntity _existingEntity;
    private UpdateWeatherForecastRequest _updateRequest;
    private string _testUsername;

    protected override void GivenThat()
    {
        var randomizerString = new RandomAlphanumericStringGenerator();
        var randomizerDate = new RandomDateTimeGenerator();
        var randomizerInteger = new RandomIntegerGenerator();

        _testId = Guid.NewGuid();
        _testUsername = randomizerString.GenerateValue();
        
        // Set up Thread Principal for username
        var identity = new GenericIdentity(_testUsername);
        var principal = new GenericPrincipal(identity, null);
        Thread.CurrentPrincipal = principal;
        
        _existingEntity = new WeatherForecastEntity(_testId)
        {
            Summary = "Old Summary",
            Temperature = 20,
            Date = DateTime.UtcNow.AddDays(-1)
        };

        _updateRequest = new UpdateWeatherForecastRequest
        {
            Summary = randomizerString.GenerateValue(),
            TemperatureInCelsius = randomizerInteger.GenerateValue(),
            Date = randomizerDate.GenerateValue()
        };

        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(
            It.Is<GetWeatherForecastByIdQuery>(q => q.Id == _testId)))
            .ReturnsAsync(_existingEntity);

        _commandDispatcher = Dep<ICommandDispatcher>();
        _commandDispatcher.Setup(x => x.Execute(It.IsAny<UpdateWeatherForecastCommand>()))
            .Returns(Task.CompletedTask);

        _eventDispatcher = Dep<IEventDispatcher>();
        _eventDispatcher.Setup(x => x.Raise(It.IsAny<WeatherForecastUpdatedEvent>()))
            .Returns(Task.CompletedTask);

        _backgroundTaskQueue = Dep<IBackgroundTaskQueue>();
        _backgroundTaskQueue.Setup(x => x.Queue(It.IsAny<string>(), It.IsAny<Func<CancellationToken, Task>>()));
    }

    protected override void WhenIRun()
    {
        CreateSut().Execute(_testId, _updateRequest).Wait();
    }

    [TestMethod]
    public void Should_Query_For_Entity()
    {
        _queryDispatcher.Verify(x => x.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(
            It.Is<GetWeatherForecastByIdQuery>(q => q.Id == _testId)), Times.Once);
    }

    [TestMethod]
    public void Should_Dispatch_Update_Command()
    {
        _commandDispatcher.Verify(x => x.Execute(It.Is<UpdateWeatherForecastCommand>(cmd =>
            cmd.WeatherForecast.Summary == _updateRequest.Summary &&
            cmd.WeatherForecast.Temperature == _updateRequest.TemperatureInCelsius &&
            cmd.Username == _testUsername
        )), Times.Once);
    }

    [TestMethod]
    public void Should_Queue_Background_Task()
    {
        var queueMessage = $"Publishing WeatherForecastUpdatedEvent for {_testId}";
        _backgroundTaskQueue.Verify(
            x => x.Queue(It.Is<string>(msg => msg.Equals(queueMessage)), It.IsAny<Func<CancellationToken, Task>>()),
            Times.Once);
    }
}
