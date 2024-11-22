using System;
using AGTec.Common.Repository.Document;
using AGTec.Common.Repository.Document.Configuration;
using AGTec.Common.Repository.Search;
using AGTec.Common.Repository.Search.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace uServiceDemo.Document;

public static class Module
{
    public static IServiceCollection AddDocumentModule(this IServiceCollection services,
        IConfiguration configuration)

    {
        // DocDb Configuration
        var documentDBConfiguration =
            configuration.GetSection(DocumentDBConfiguration.ConfigSectionName).Get<DocumentDBConfiguration>();

        if (documentDBConfiguration.IsValid() == false)
            throw new Exception($"Configuration section '{DocumentDBConfiguration.ConfigSectionName}' not found.");

        services.AddSingleton<IDocumentDBConfiguration>(documentDBConfiguration);

        // SearchDb Configuration
        var searchDbConfiguration = configuration.GetSection(SearchDbConfiguration.ConfigSectionName)
            .Get<SearchDbConfiguration>();

        if (searchDbConfiguration.IsValid() == false)
            throw new Exception($"Configuration section '{SearchDbConfiguration.ConfigSectionName}' not found.");

        services.AddSingleton<ISearchDbConfiguration>(searchDbConfiguration);

        // Context
        services.AddTransient<IDocumentContext, DocumentContext>();
        services.AddTransient<ISearchContext, SearchContext>();

        // Repositories
        services.AddTransient<IWeatherForecastDocReadOnlyRepository, WeatherForecastDocReadOnlyRepository>();
        services.AddTransient<IWeatherForecastDocRepository, WeatherForecastDocRepository>();
        services.AddTransient<IWeatherForecastSearchReadOnlyRepository, WeatherForecastSearchReadOnlyRepository>();
        services.AddTransient<IWeatherForecastSearchRepository, WeatherForecastSearchRepository>();

        return services;
    }
}