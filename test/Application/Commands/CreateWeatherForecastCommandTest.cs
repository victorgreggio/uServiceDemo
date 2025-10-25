using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Commands;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.Commands;

[TestClass]
public class CreateWeatherForecastCommandTest
{
    [TestMethod]
    public void Should_Create_Command_With_Entity_And_Username()
    {
        var entity = new WeatherForecastEntity(Guid.NewGuid())
        {
            Summary = "Test",
            Temperature = 25
        };
        var username = "testuser";

        var command = new CreateWeatherForecastCommand(entity, username);

        Assert.AreEqual(entity, command.WeatherForecast);
        Assert.AreEqual(username, command.Username);
    }

    [TestMethod]
    public void Should_Allow_Null_Username()
    {
        var entity = new WeatherForecastEntity(Guid.NewGuid());

        var command = new CreateWeatherForecastCommand(entity, null);

        Assert.IsNull(command.Username);
    }
}
