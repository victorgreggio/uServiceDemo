using System.Linq;
using System.Threading.Tasks;
using AGTec.Common.CQRS.CommandHandlers;
using AGTec.Common.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
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

    public async Task Execute(DeleteWeatherForecastCommand command)
    {
        var entity = await _repository
            .Select(x => x.Id == command.Id)
            .FirstOrDefaultAsync();
            
        if (entity != null)
        {
            await _repository.Delete(entity);
        }
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}
