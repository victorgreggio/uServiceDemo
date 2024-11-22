using System.Threading.Tasks;
using AGTec.Common.CQRS.CommandHandlers;
using uServiceDemo.Application.Commands;
using uServiceDemo.Infrastructure.Repositories;

namespace uServiceDemo.Application.CommandHandlers;

internal class CreateWeatherForecastCommandHandler : ICommandHandler<CreateWeatherForecastCommand>
{
    private readonly IWeatherForecastRepository _repository;

    public CreateWeatherForecastCommandHandler(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public Task Execute(CreateWeatherForecastCommand command)
    {
        var entity = command.WeatherForecast;
        entity.UpdatedBy = command.Username;
        return _repository.Insert(entity);
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}