using AGTec.Microservice;
using Microsoft.Extensions.Hosting;

namespace uServiceDemo.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return HostBuilderFactory.CreateHostBuilder<Startup>(args);
    }
}