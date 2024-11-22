using AGTec.Common.Repository.Document;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Document;

public class WeatherForecastDocReadOnlyRepository : ReadOnlyRepository<WeatherForecastDoc>,
    IWeatherForecastDocReadOnlyRepository
{
    public WeatherForecastDocReadOnlyRepository(IDocumentContext context)
        : base(context)
    {
    }
}