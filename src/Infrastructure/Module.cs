using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using uServiceDemo.Infrastructure.Repositories;
using uServiceDemo.Infrastructure.Repositories.Context;

namespace uServiceDemo.Infrastructure;

public static class Module
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        //DbContext
        services.AddDbContext<WeatherForecastDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString(Constants.Database.DabaseConnectionString),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(Module).Assembly.FullName)));


        // Repositories
        services.AddTransient<IWeatherForecastReadOnlyRepository, WeatherForecastReadOnlyRepository>();
        services.AddTransient<IWeatherForecastRepository, WeatherForecastRepository>();
        services.AddTransient<IWindReadOnlyRepository, WindReadOnlyRepository>();
        services.AddTransient<IWindRepository, WindRepository>();

        return services;
    }
}