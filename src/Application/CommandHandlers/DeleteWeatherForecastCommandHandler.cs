using System.Threading.Tasks;
using AGTec.Common.CQRS.CommandHandlers;
using uServiceDemo.Application.Commands;
using uServiceDemo.Infrastructure.Repositories;

namespace uServiceDemo.Application.CommandHandlers;

internal class DeleteWeatherForecastCommandHandler : ICommandHandler<DeleteWeatherForecastCommand>
{
    private readonly IWeatherForecastRepository _repository;

    public DeleteWeatherForecastCommandHandler(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public Task Execute(DeleteWeatherForecastCommand command)
    {
        return _repository.Delete(command.Id);
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}
