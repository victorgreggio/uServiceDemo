using System;
using System.Threading.Tasks;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.GetWeatherForecast.V1;
using uServiceDemo.Contracts;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class GetWeatherForecastUseCaseTest : AutoMockSpecification<GetWeatherForecastUseCase, IGetWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private Guid _testId;
    private WeatherForecastEntity _existingEntity;
    private WeatherForecast _result;

    protected override Task GivenThat()
    {
        _testId = Guid.NewGuid();
        _existingEntity = new WeatherForecastEntity(_testId)
        {
            Summary = "Cloudy",
            Temperature = 18,
            Date = DateTime.UtcNow
        };

        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(
            It.Is<GetWeatherForecastByIdQuery>(q => q.Id == _testId)))
            .ReturnsAsync(_existingEntity);

        return Task.CompletedTask;
    }

    protected override async Task WhenIRun() => _result = await CreateSut().Execute(_testId);

    [TestMethod]
    public void Should_Query_For_Entity()
    {
        _queryDispatcher.Verify(x => x.Execute<GetWeatherForecastByIdQuery, WeatherForecastEntity>(
            It.Is<GetWeatherForecastByIdQuery>(q => q.Id == _testId)), Times.Once);
    }

    [TestMethod]
    public void Should_Return_Mapped_Result()
    {
        Assert.IsNotNull(_result);
        Assert.AreEqual(_testId, _result.Id);
        Assert.AreEqual(_existingEntity.Summary, _result.Summary);
        Assert.AreEqual(_existingEntity.Temperature, _result.TemperatureInCelsius);
    }
}
