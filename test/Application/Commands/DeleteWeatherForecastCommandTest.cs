using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uServiceDemo.Application.Commands;

namespace uServiceDemo.Application.Test.Commands;

[TestClass]
public class DeleteWeatherForecastCommandTest
{
    [TestMethod]
    public void Should_Create_Command_With_Id_And_Username()
    {
        var id = Guid.NewGuid();
        var username = "deleter";

        var command = new DeleteWeatherForecastCommand(id, username);

        Assert.AreEqual(id, command.Id);
        Assert.AreEqual(username, command.Username);
    }

    [TestMethod]
    public void Should_Allow_Null_Username()
    {
        var id = Guid.NewGuid();

        var command = new DeleteWeatherForecastCommand(id, null);

        Assert.IsNull(command.Username);
    }

    [TestMethod]
    public void Should_Accept_Empty_Guid()
    {
        var command = new DeleteWeatherForecastCommand(Guid.Empty, "testuser");

        Assert.AreEqual(Guid.Empty, command.Id);
    }
}
