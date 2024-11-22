using System.Threading.Tasks;
using AGTec.Common.CQRS.CommandHandlers;
using uServiceDemo.Application.Commands;
using uServiceDemo.Infrastructure.Repositories;

namespace uServiceDemo.Application.CommandHandlers;

internal class UpdateWeatherForecastCommandHandler : ICommandHandler<UpdateWeatherForecastCommand>
{
    private readonly IWeatherForecastRepository _repository;

    public UpdateWeatherForecastCommandHandler(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public Task Execute(UpdateWeatherForecastCommand command)
    {
        var entity = command.WeatherForecast;
        entity.UpdatedBy = command.Username;
        return _repository.Update(entity);
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}