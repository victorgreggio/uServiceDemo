using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Queries;

namespace uServiceDemo.Application.Test.Queries;

[TestClass]
public class SearchWeatherForecastQueryTest
{
    [TestMethod]
    public void Should_Create_Query_With_Term()
    {
        var term = "sunny";
        
        var query = new SearchWeatherForecastQuery(term);
        
        Assert.AreEqual(term, query.Term);
    }

    [TestMethod]
    public void Should_Allow_Empty_Term()
    {
        var query = new SearchWeatherForecastQuery(string.Empty);
        
        Assert.AreEqual(string.Empty, query.Term);
    }

    [TestMethod]
    public void Should_Allow_Null_Term()
    {
        var query = new SearchWeatherForecastQuery(null);
        
        Assert.IsNull(query.Term);
    }
}
