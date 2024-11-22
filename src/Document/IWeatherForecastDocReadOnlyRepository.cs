using AGTec.Common.Repository.Document;
using uServiceDemo.Document.Entities;

namespace uServiceDemo.Document;

public interface IWeatherForecastDocReadOnlyRepository : IReadOnlyRepository<WeatherForecastDoc>
{
}