var builder = DistributedApplication.CreateBuilder(args);

var mongodb = builder.AddMongoDB("mongo").WithDataVolume().AddDatabase("uServiceDemo");


var apiService = builder.AddProject<Projects.uServiceDemo_Api>("apiService")
    .WithReference(mongodb)
    .WaitFor(mongodb);


var workerService = builder.AddProject<Projects.uServiceDemo_Worker>("workerService");


builder.Build().Run();