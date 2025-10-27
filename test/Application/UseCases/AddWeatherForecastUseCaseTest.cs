using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AGTec.Common.BackgroundTaskQueue;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Randomizer.Impl;
using AGTec.Common.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Commands;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.UseCases.AddWeatherForecast.V1;
using uServiceDemo.Contracts.Requests;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class
    AddWeatherForecastUseCaseTest : AutoMockSpecification<AddWeatherForecastUseCase, IAddWeatherForecastUseCase>
{
    private Mock<IBackgroundTaskQueue> _backgroundTaskQueue;

    private Mock<ICommandDispatcher> _commandDispatcher;

    private DateTime _forecastDate;
    private string _forecastSummary;
    private int _forecastTemperature;
    private Guid _resultId;
    private string _testUsername;

    protected override Task GivenThat()
    {
        var randomizerString = new RandomAlphanumericStringGenerator();
        var randomizerDate = new RandomDateTimeGenerator();
        var randomizerInteger = new RandomIntegerGenerator();

        // Test data
        _forecastDate = randomizerDate.GenerateValue();
        _forecastSummary = randomizerString.GenerateValue();
        _forecastTemperature = randomizerInteger.GenerateValue();
        _testUsername = randomizerString.GenerateValue();

        // Set up Thread Principal for username
        var identity = new GenericIdentity(_testUsername);
        var principal = new GenericPrincipal(identity, null);
        Thread.CurrentPrincipal = principal;

        // Create AutoMapper instance using Application's configuration
        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        // CommandDispatcher Mock
        _commandDispatcher = Dep<ICommandDispatcher>();
        _commandDispatcher.Setup(x => x.Execute<CreateWeatherForecastCommand>(It.IsAny<CreateWeatherForecastCommand>()))
            .Returns(Task.CompletedTask);

        // BackgroundTaskQueue Mock
        _backgroundTaskQueue = Dep<IBackgroundTaskQueue>();
        _backgroundTaskQueue.Setup(x => x.Queue(It.IsAny<string>(), It.IsAny<Func<CancellationToken, Task>>()));

        return Task.CompletedTask;
    }

    protected override async Task WhenIRun() => _resultId = await CreateSut()
            .Execute(
                new AddWeatherForecastRequest
                { Date = _forecastDate, Summary = _forecastSummary, TemperatureInCelsius = _forecastTemperature });

    [TestMethod]
    public void Should_Return_Valid_Guid()
    {
        Assert.AreNotEqual(Guid.Empty, _resultId);
    }

    [TestMethod]
    public void Should_Dispatch_Command_With_Properly_Mapped_Entity()
    {
        _commandDispatcher.Verify(x => x.Execute<CreateWeatherForecastCommand>(It.IsAny<CreateWeatherForecastCommand>()), Times.Once);
    }

    [TestMethod]
    public void Should_Queue_Background_Task()
    {
        _backgroundTaskQueue.Verify(
            x => x.Queue(It.Is<string>(msg => msg.StartsWith("Publishing WeatherForecastCreatedEvent for ")),
                It.IsAny<Func<CancellationToken, Task>>()),
            Times.Once);
    }
}
