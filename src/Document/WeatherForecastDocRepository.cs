using AGTec.Common.Repository.Document;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Document;

public class WeatherForecastDocRepository : Repository<WeatherForecastDoc>, IWeatherForecastDocRepository
{
    public WeatherForecastDocRepository(IDocumentContext context)
        : base(context)
    {
    }
}