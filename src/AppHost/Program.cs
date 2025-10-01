using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("Postgres").AddDatabase("WeatherforecastDB");
var mongodb = builder.AddMongoDB("MongoDB").AddDatabase("MongoWeatherforecastDocumentDB");
var azureServiceBus = builder.AddAzureServiceBus("AzureServiceBus");
var elasticsearch = builder.AddElasticsearch("Elasticsearch");

// Api
var api = builder.AddProject<Projects.uServiceDemo_Api>("api")
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
var ui = builder.AddProject<Projects.uServiceDemo_UI>("ui");

builder.AddYarp("ApiGateway")
    .WithHostPort(8081)
    .WithConfiguration(yarp =>
    {
        yarp.AddRoute("/api/{**catch-all}", api)
            .WithTransformPathRemovePrefix("/api");
    })
    .WithReference(ui);


builder.Build().Run();