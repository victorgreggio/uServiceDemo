using Aspire.Hosting.Yarp.Transforms;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Configure to accept self-signed certificates in development
if (builder.Environment.IsDevelopment())
{
    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
}

var postgres = builder.AddPostgres("Postgres").AddDatabase("WeatherforecastDB");
var mongodb = builder.AddMongoDB("MongoDB").AddDatabase("MongoWeatherforecastDB");
var azureServiceBus = builder.AddAzureServiceBus("AzureServiceBus");
var elasticsearch = builder.AddElasticsearch("Elasticsearch");

// Api
var api = builder.AddProject<Projects.uServiceDemo_Api>("api")
    .WithHttpEndpoint(name: "apihttp")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(mongodb)
    .WaitFor(mongodb)
    .WithReference(azureServiceBus)
    .WaitFor(azureServiceBus)
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch);

// Worker
builder.AddProject<Projects.uServiceDemo_Worker>("worker")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(mongodb)
    .WaitFor(mongodb)
    .WithReference(azureServiceBus)
    .WaitFor(azureServiceBus)
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch);

// UI
builder.AddProject<Projects.uServiceDemo_UI>("ui")
    .WithReference(api.GetEndpoint("apihttp"));


builder.Build().Run();