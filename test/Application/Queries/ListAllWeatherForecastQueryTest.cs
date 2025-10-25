using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Queries;

namespace uServiceDemo.Application.Test.Queries;

[TestClass]
public class ListAllWeatherForecastQueryTest
{
    [TestMethod]
    public void Should_Create_Query()
    {
        var query = new ListAllWeatherForecastQuery();
        
        Assert.IsNotNull(query);
    }
}
