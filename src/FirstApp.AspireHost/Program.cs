var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FirstApp_Web>("web");

builder.Build().Run();
