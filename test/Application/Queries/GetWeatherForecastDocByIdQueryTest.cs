using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Queries;

namespace uServiceDemo.Application.Test.Queries;

[TestClass]
public class GetWeatherForecastDocByIdQueryTest
{
    [TestMethod]
    public void Should_Create_Query_With_Id()
    {
        var id = Guid.NewGuid();
        
        var query = new GetWeatherForecastDocByIdQuery(id);
        
        Assert.AreEqual(id, query.Id);
    }
}
