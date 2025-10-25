using AGTec.Services.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using uServiceDemo.Api;
using uServiceDemo.Application;
using uServiceDemo.Infrastructure.Repositories.Context;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddElasticsearchClient(connectionName: "Elasticsearch");
        builder.AddMongoDBClient(connectionName: "MongoWeatherforecastDocumentDB");

        builder.AddServiceDefaults<WeatherForecastDbContext>();
        builder.Services.AddApplicationModule(builder.Configuration);

        var authConfig = builder.Configuration.GetSection("Authentication");
        var authority = authConfig["Authority"] ?? throw new System.Exception("Invalid configuration. Could not find Authentication:Authority");
        var audience = authConfig["Audience"] ?? throw new System.Exception("Invalid configuration. Could not find Authentication:Audience");
        var requireHttpsMetadata = !builder.Environment.IsDevelopment();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidIssuer = authority,
                    ValidAudience = audience
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseServiceDefaults<WeatherForecastDbContext>();
        app.MapEndpoints();

        app.Run();
    }
}