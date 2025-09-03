var builder = DistributedApplication.CreateBuilder(args);

var insights = builder.AddAzureApplicationInsights("ApplicationInsights");
var postgres = builder.AddPostgres("Postgres").AddDatabase("WeatherforecastDB");
var mongodb = builder.AddMongoDB("MongoDB").AddDatabase("MongoWeatherforecastDocumentDB");
var azureServiceBus = builder.AddAzureServiceBus("AzureServiceBus");
var elasticsearch = builder.AddElasticsearch("Elasticsearch");

// Api
builder.AddProject<Projects.uServiceDemo_Api>("api")
    .WithReference(insights)
    .WaitFor(insights)
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
    .WithReference(insights)
    .WaitFor(insights)
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(mongodb)
    .WaitFor(mongodb)
    .WithReference(azureServiceBus)
    .WaitFor(azureServiceBus)
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch);


builder.Build().Run();