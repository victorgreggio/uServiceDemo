using System.Collections.Generic;
using System.Threading.Tasks;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Exceptions;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.SearchWeatherForecast.V1;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
[ExceptionSpecification]
public class SearchWeatherForecastUseCaseTest_NoResults : AutoMockSpecification<SearchWeatherForecastUseCase, ISearchWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private string _searchTerm;

    protected override Task GivenThat()
    {
        _searchTerm = "nonexistent";

        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<SearchWeatherForecastQuery, IEnumerable<WeatherForecastDoc>>(
            It.IsAny<SearchWeatherForecastQuery>()))
            .ReturnsAsync(new List<WeatherForecastDoc>());

        return Task.CompletedTask;
    }

    protected override async Task WhenIRun() => await CreateSut().Execute(_searchTerm);

    [TestMethod]
    public void Should_Throw_NotFoundException() => AssertExceptionThrown<NotFoundException>();
}
