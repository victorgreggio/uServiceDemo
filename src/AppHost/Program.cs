using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
}

var postgres = builder.AddPostgres("Postgres").AddDatabase("WeatherforecastDB");
var mongodb = builder.AddMongoDB("MongoDB").AddDatabase("MongoWeatherforecastDocumentDB");
var rabbitmq = builder.AddRabbitMQ("RabbitMQ");
        
var elasticsearch = builder.AddElasticsearch("Elasticsearch");

// Api
var api = builder.AddProject<Projects.uServiceDemo_Api>("api")
    .WithHttpEndpoint(name: "apihttp")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(mongodb)
    .WaitFor(mongodb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch);

// Worker
builder.AddProject<Projects.uServiceDemo_Worker>("worker")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(mongodb)
    .WaitFor(mongodb)
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(elasticsearch)
    .WaitFor(elasticsearch);

// UI
builder.AddProject<Projects.uServiceDemo_UI>("ui")
    .WithReference(api.GetEndpoint("apihttp"))
    .WaitFor(api);


builder.Build().Run();