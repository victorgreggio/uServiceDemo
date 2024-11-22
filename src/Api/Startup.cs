using AGTec.Microservice;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uServiceDemo.Application;
using uServiceDemo.Infrastructure.Repositories.Context;

namespace uServiceDemo.Api;

public class Startup
{
    public Startup(IConfiguration configuration, IHostEnvironment hostEnv)
    {
        Configuration = configuration;
        HostEnv = hostEnv;
    }

    public IConfiguration Configuration { get; }
    public IHostEnvironment HostEnv { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAGTecMicroservice<WeatherForecastDbContext>(Configuration, HostEnv);
        services.AddApplicationModule(Configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseAGTecMicroservice<WeatherForecastDbContext>(HostEnv);
    }
}