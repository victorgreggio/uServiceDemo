using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Commands;
using uServiceDemo.Domain.Entities;

namespace uServiceDemo.Application.Test.Commands;

[TestClass]
public class UpdateWeatherForecastCommandTest
{
    [TestMethod]
    public void Should_Create_Command_With_Entity_And_Username()
    {
        var entity = new WeatherForecastEntity(Guid.NewGuid())
        {
            Summary = "Updated",
            Temperature = 30
        };
        var username = "updater";

        var command = new UpdateWeatherForecastCommand(entity, username);

        Assert.AreEqual(entity, command.WeatherForecast);
        Assert.AreEqual(username, command.Username);
    }

    [TestMethod]
    public void Should_Allow_Null_Username()
    {
        var entity = new WeatherForecastEntity(Guid.NewGuid());

        var command = new UpdateWeatherForecastCommand(entity, null);

        Assert.IsNull(command.Username);
    }
}
