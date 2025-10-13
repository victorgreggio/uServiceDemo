using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using uServiceDemo.UI;
using uServiceDemo.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with API base URL from configuration
var apiBaseUrl = builder.Configuration["services:apihttp:http:0"] ?? 
                 builder.Configuration["services:api:http:0"] ??
                 builder.Configuration["ApiBaseUrl"] ?? 
                 "http://localhost:5000";

Console.WriteLine($"Using API Base URL: {apiBaseUrl}");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

await builder.Build().RunAsync();
