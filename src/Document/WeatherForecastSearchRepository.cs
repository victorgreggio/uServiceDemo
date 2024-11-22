using AGTec.Common.Repository.Search;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Document;

public class WeatherForecastSearchRepository : Repository<WeatherForecastDoc, ISearchContext>,
    IWeatherForecastSearchRepository
{
    public WeatherForecastSearchRepository(ISearchContext context)
        : base(context)
    {
    }
}