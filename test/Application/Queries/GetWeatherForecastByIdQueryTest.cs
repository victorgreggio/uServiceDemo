using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Queries;

namespace uServiceDemo.Application.Test.Queries;

[TestClass]
public class GetWeatherForecastByIdQueryTest
{
    [TestMethod]
    public void Should_Create_Query_With_Id()
    {
        var id = Guid.NewGuid();
        
        var query = new GetWeatherForecastByIdQuery(id);
        
        Assert.AreEqual(id, query.Id);
    }

    [TestMethod]
    public void Should_Accept_Empty_Guid()
    {
        var query = new GetWeatherForecastByIdQuery(Guid.Empty);
        
        Assert.AreEqual(Guid.Empty, query.Id);
    }
}
