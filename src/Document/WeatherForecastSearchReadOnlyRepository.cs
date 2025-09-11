using AGTec.Common.Repository.Search;
using Elastic.Clients.Elasticsearch;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Document;

public class WeatherForecastSearchReadOnlyRepository : ReadOnlyRepository<WeatherForecastDoc>,
    IWeatherForecastSearchReadOnlyRepository
{
    public WeatherForecastSearchReadOnlyRepository(ElasticsearchClient client)
        : base(client)
    {
    }
}