using AGTec.Common.Repository.Search;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Document;

public interface IWeatherForecastSearchRepository : IRepository<WeatherForecastDoc, ISearchContext>
{
}