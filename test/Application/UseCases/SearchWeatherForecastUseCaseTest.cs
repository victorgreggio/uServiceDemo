using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGTec.Common.CQRS.Dispatchers;
using AGTec.Common.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Application.Queries;
using uServiceDemo.Application.UseCases.SearchWeatherForecast.V1;
using uServiceDemo.Contracts;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Application.Test.UseCases;

[TestClass]
public class SearchWeatherForecastUseCaseTest : AutoMockSpecification<SearchWeatherForecastUseCase, ISearchWeatherForecastUseCase>
{
    private Mock<IQueryDispatcher> _queryDispatcher;
    private string _searchTerm;
    private IEnumerable<WeatherForecastDoc> _documents;
    private IEnumerable<WeatherForecast> _result;

    protected override Task GivenThat()
    {
        _searchTerm = "sunny";
        var doc1Id = Guid.NewGuid();
        var doc2Id = Guid.NewGuid();
        _documents =
        [
            new(doc1Id) { Summary = "Sunny Day", Temperature = 25, Date = DateTime.UtcNow },
            new(doc2Id) { Summary = "Mostly Sunny", Temperature = 22, Date = DateTime.UtcNow }
        ];

        AutoMocker.SetInstance(MapConfig.GetMapperConfiguration().CreateMapper());

        _queryDispatcher = Dep<IQueryDispatcher>();
        _queryDispatcher.Setup(x => x.Execute<SearchWeatherForecastQuery, IEnumerable<WeatherForecastDoc>>(
            It.Is<SearchWeatherForecastQuery>(q => q.Term == _searchTerm)))
            .ReturnsAsync(_documents);

        return Task.CompletedTask;
    }

    protected override async Task WhenIRun() => _result = await CreateSut().Execute(_searchTerm);

    [TestMethod]
    public void Should_Query_With_Search_Term()
    {
        _queryDispatcher.Verify(x => x.Execute<SearchWeatherForecastQuery, IEnumerable<WeatherForecastDoc>>(
            It.Is<SearchWeatherForecastQuery>(q => q.Term == _searchTerm)), Times.Once);
    }

    [TestMethod]
    public void Should_Return_Correct_Count()
    {
        Assert.AreEqual(_documents.Count(), _result.Count());
    }

    [TestMethod]
    public void Should_Map_All_Documents()
    {
        var resultList = _result.ToList();
        var docList = _documents.ToList();

        for (int i = 0; i < docList.Count; i++)
        {
            Assert.AreEqual(docList[i].Id, resultList[i].Id);
            Assert.AreEqual(docList[i].Summary, resultList[i].Summary);
            Assert.AreEqual(docList[i].Temperature, resultList[i].TemperatureInCelsius);
        }
    }
}
