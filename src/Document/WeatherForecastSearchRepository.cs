using AGTec.Common.Repository.Search;
using Elastic.Clients.Elasticsearch;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Document;

public class WeatherForecastSearchRepository : Repository<WeatherForecastDoc>,
    IWeatherForecastSearchRepository
{
    public WeatherForecastSearchRepository(ElasticsearchClient client)
        : base(client)
    {
    }
}