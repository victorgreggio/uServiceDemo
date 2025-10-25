using AGTec.Common.Base.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using uServiceDemo.UI;
using uServiceDemo.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with API base URL from configuration
// Priority: Aspire service discovery > ApiBaseUrl from appsettings
var apiBaseUrl = builder.Configuration["services:apihttp:http:0"] ?? 
                 builder.Configuration["services:api:http:0"] ??
                 builder.Configuration["ApiBaseUrl"] ?? 
                 throw new InvalidOperationException("API Base URL not configured. Please configure 'ApiBaseUrl' in appsettings.json or use Aspire service discovery.");

System.Console.WriteLine($"API Base URL: {apiBaseUrl}");

// Read OAuth configuration from appsettings.{Environment}.json
var authConfig = builder.Configuration.GetSection("Authentication");
var authority = authConfig["Authority"] ?? throw new InvalidOperationException("Authentication:Authority not configured in appsettings.json");
var clientId = authConfig["ClientId"] ?? throw new InvalidOperationException("Authentication:ClientId not configured in appsettings.json");
var responseType = authConfig["ResponseType"] ?? throw new InvalidOperationException("Authentication:ResponseType not configured in appsettings.json");
var scopes = authConfig.GetSection("Scopes").Get<string[]>() ?? throw new InvalidOperationException("Authentication:Scopes not configured in appsettings.json");

System.Console.WriteLine($"Authentication Configuration:");
System.Console.WriteLine($"  Authority: {authority}");
System.Console.WriteLine($"  ClientId: {clientId}");
System.Console.WriteLine($"  ResponseType: {responseType}");
System.Console.WriteLine($"  Scopes: {string.Join(", ", scopes)}");

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = authority;
    options.ProviderOptions.ClientId = clientId;
    options.ProviderOptions.ResponseType = responseType;
    scopes.ForEach(scope => options.ProviderOptions.DefaultScopes.Add(scope));
});

// Read authorized URLs from configuration
var authorizedUrls = builder.Configuration.GetSection("Api:BaseUrls").Get<string[]>() 
    ?? throw new InvalidOperationException("Api:BaseUrls not configured in appsettings.json");

System.Console.WriteLine($"Authorized URLs: {string.Join(", ", authorizedUrls)}");

// Register the UnauthorizedResponseHandler
builder.Services.AddScoped<UnauthorizedResponseHandler>();

builder.Services.AddHttpClient("API", 
    client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<UnauthorizedResponseHandler>()
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>();
        handler.ConfigureHandler(
            authorizedUrls: authorizedUrls,
            scopes: scopes);
        return handler;
    });

builder.Services.AddScoped(sp => 
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

await builder.Build().RunAsync();
