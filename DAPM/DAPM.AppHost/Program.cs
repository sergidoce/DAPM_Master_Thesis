var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DAPM_ClientApi>("dapm-clientapi");

builder.AddProject<Projects.DAPM_RepositoryMS_Api>("dapm-repositoryms-api");

builder.AddProject<Projects.DAPM_ResourceRegistryMS_Api>("dapm-resourceregistryms-api");

builder.Build().Run();
