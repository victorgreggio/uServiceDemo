using System.Linq;
using AGTec.Common.Base.Extensions;
using AGTec.Common.CQRS.CommandHandlers;
using AGTec.Common.CQRS.Messaging.AzureServiceBus;
using AGTec.Common.CQRS.Messaging.ProtoBufSerializer;
using AGTec.Common.CQRS.QueryHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using uServiceDemo.Application.Mappers;
using uServiceDemo.Document;
using uServiceDemo.Infrastructure;

namespace uServiceDemo.Application;

public static class Module
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services,
        IConfiguration configuration)

    {
        // Infrastructure
        services.AddInfrastructureModule(configuration);

        // DocumentDB(MongoDB)
        services.AddDocumentModule();

        // CQRS
        var azuerServiceBusConnectionString = configuration.GetConnectionString("AzureServiceBus");
        services.AddProtoBufMessagingSerializer();
        services.AddCQRSWithMessaging(azuerServiceBusConnectionString);

        // Mappers
        services.AddSingleton(MapConfig.GetMapperConfiguration().CreateMapper());

        // Commands & Queries handlers
        services.AddCommands();
        services.AddQueries();

        // UseCases
        services.AddUseCases();

        return services;
    }

    #region UseCases

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        typeof(Module).Assembly
            .GetTypes()
            .Where(t => t.IsClass && t.IsAbstract == false &&
                        t.IsNested == false && t.Namespace.Contains("UseCases"))
            .ForEach(type =>
            {
                var interfaceType = type.GetInterfaces().FirstOrDefault();
                services.AddTransient(interfaceType, type);
            });
        return services;
    }

    #endregion

    #region Commands

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        typeof(Module).Assembly
            .GetTypes()
            .Where(t => t.IsClass && t.IsAbstract == false)
            .ForEach(type =>
            {
                type.GetInterfaces().ForEach(i =>
                {
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                    {
                        var interfaceType = typeof(ICommandHandler<>).MakeGenericType(i.GetGenericArguments());
                        services.AddTransient(interfaceType, type);
                    }
                });
            });
        return services;
    }

    #endregion

    #region Queries

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        typeof(Module).Assembly
            .GetTypes()
            .Where(t => t.IsClass && t.IsAbstract == false)
            .ForEach(type =>
            {
                type.GetInterfaces().ForEach(i =>
                {
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                    {
                        var interfaceType = typeof(IQueryHandler<,>).MakeGenericType(i.GetGenericArguments());
                        services.AddTransient(interfaceType, type);
                    }
                });
            });

        return services;
    }

    #endregion
}