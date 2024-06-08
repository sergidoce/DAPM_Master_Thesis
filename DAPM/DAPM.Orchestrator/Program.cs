using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using RabbitMQLibrary.Messages.ResourceRegistry;
using DAPM.Orchestrator.Consumers.StartProcessConsumers;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using DAPM.Orchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry;
using DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQueueing(new QueueingConfigurationSettings
{
    RabbitMqConsumerConcurrency = 5,
    RabbitMqHostname = "rabbitmq",
    RabbitMqPort = 5672,
    RabbitMqPassword = "guest",
    RabbitMqUsername = "guest"
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

//StartRequests
builder.Services.AddQueueMessageConsumer<GetOrganizationsRequestConsumer, GetOrganizationsRequest>();
builder.Services.AddQueueMessageConsumer<GetRepositoriesRequestConsumer, GetRepositoriesRequest>();
builder.Services.AddQueueMessageConsumer<GetResourcesRequestConsumer, GetResourcesRequest>();
builder.Services.AddQueueMessageConsumer<GetPipelinesRequestConsumer, GetPipelinesRequest>();
builder.Services.AddQueueMessageConsumer<PostResourceRequestConsumer, PostResourceRequest>();
builder.Services.AddQueueMessageConsumer<PostRepositoryRequestConsumer, PostRepositoryRequest>();
builder.Services.AddQueueMessageConsumer<PostPipelineRequestConsumer, PostPipelineRequest>();
builder.Services.AddQueueMessageConsumer<RegisterPeerRequestConsumer, RegisterPeerRequest>();
builder.Services.AddQueueMessageConsumer<GetResourceFilesRequestConsumer, GetResourceFilesRequest>();





//ServicesResults
builder.Services.AddQueueMessageConsumer<GetOrgsFromRegistryResultConsumer, GetOrganizationsResultMessage>();
builder.Services.AddQueueMessageConsumer<GetReposFromRegistryResultConsumer, GetRepositoriesResultMessage>();
builder.Services.AddQueueMessageConsumer<GetResourcesFromRegistryResultConsumer, GetResourcesResultMessage>();
builder.Services.AddQueueMessageConsumer<PostResourceToRepoResultConsumer, PostResourceToRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<PostResourceToRegistryResultConsumer, PostResourceToRegistryResultMessage>();
builder.Services.AddQueueMessageConsumer<PostRepoToRepoResultConsumer, PostRepoToRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<PostRepoToRegistryResultConsumer, PostRepoToRegistryResultMessage>();
builder.Services.AddQueueMessageConsumer<PostPipelineToRepoResultConsumer, PostPipelineToRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<GetPipelinesFromRepoResultConsumer, GetPipelinesFromRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<PostPipelineToRegistryResultConsumer, PostPipelineToRegistryResultMessage>();
builder.Services.AddQueueMessageConsumer<GetPipelinesFromRegistryResultConsumer, GetPipelinesResultMessage>();
builder.Services.AddQueueMessageConsumer<GetResourceFilesFromRepoResultConsumer, GetResourceFilesFromRepoResultMessage>();








builder.Services.AddSingleton<IOrchestratorEngine, OrchestratorEngine>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
