using System;
using AGTec.Common.Repository.Document;
using AGTec.Common.Repository.Document.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace uServiceDemo.Document;

public static class Module
{
    public static IServiceCollection AddDocumentModule(this IServiceCollection services)
    {
        // DocDb Configuration
        var documentDBName = Constants.Database.DocumentDBName;
        var documentDBConfiguration = new DocumentDBConfiguration() { DatabaseName = documentDBName };

        if (documentDBConfiguration.IsValid() == false)
            throw new Exception($"No Document database name found in configuration.");

        services.AddSingleton<IDocumentDBConfiguration>(documentDBConfiguration);

        // Context
        services.AddSingleton<IDocumentContext, DocumentContext>();

        // Repositories
        services.AddTransient<IWeatherForecastDocReadOnlyRepository, WeatherForecastDocReadOnlyRepository>();
        services.AddTransient<IWeatherForecastDocRepository, WeatherForecastDocRepository>();
        services.AddTransient<IWeatherForecastSearchReadOnlyRepository, WeatherForecastSearchReadOnlyRepository>();
        services.AddTransient<IWeatherForecastSearchRepository, WeatherForecastSearchRepository>();

        return services;
    }
}
