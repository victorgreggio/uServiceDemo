using AGTec.Common.Repository.Search;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Document;

public class WeatherForecastSearchReadOnlyRepository : ReadOnlyRepository<WeatherForecastDoc, ISearchContext>,
    IWeatherForecastSearchReadOnlyRepository
{
    public WeatherForecastSearchReadOnlyRepository(ISearchContext context)
        : base(context)
    {
    }
}