using AGTec.Common.CQRS.Commands;
using System;

namespace uServiceDemo.Application.Commands;

public class DeleteWeatherForecastCommand : ICommand
{
    public DeleteWeatherForecastCommand(Guid id, string username)
    {
        Id = id;
        Username = username;
    }

    public Guid Id { get; }
    public string Username { get; }
}
