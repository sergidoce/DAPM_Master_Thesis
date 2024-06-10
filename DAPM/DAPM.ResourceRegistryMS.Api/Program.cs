using DAPM.ResourceRegistryMS.Api.Repositories;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using DAPM.ResourceRegistryMS.Api.Services;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using System.Text;
using DAPM.ResourceRegistryMS.Api.Consumers;
using RabbitMQLibrary.Messages.ResourceRegistry;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.WebHost.UseKestrel(o => o.Limits.MaxRequestBodySize = null);

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MultipartBoundaryLengthLimit = int.MaxValue;
    x.MultipartHeadersCountLimit = int.MaxValue;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

//RabbitMQ

builder.Services.AddQueueing(new QueueingConfigurationSettings
{
    RabbitMqConsumerConcurrency = 5,
    RabbitMqHostname = "rabbitmq",
    RabbitMqPort = 5672,
    RabbitMqPassword = "guest",
    RabbitMqUsername = "guest"
});

builder.Services.AddQueueMessageConsumer<PostRepositoryToRegistryConsumer, PostRepositoryToRegistryMessage>();
builder.Services.AddQueueMessageConsumer<PostResourceToRegistryConsumer, PostResourceToRegistryMessage>();
builder.Services.AddQueueMessageConsumer<GetOrganizationsConsumer, GetOrganizationsMessage>();
builder.Services.AddQueueMessageConsumer<GetRepositoriesConsumer, GetRepositoriesMessage>();
builder.Services.AddQueueMessageConsumer<GetResourcesConsumer, GetResourcesMessage>();
builder.Services.AddQueueMessageConsumer<GetPipelinesConsumer, GetPipelinesMessage>();
builder.Services.AddQueueMessageConsumer<PostPipelineToRegistryConsumer, PostPipelineToRegistryMessage>();
builder.Services.AddQueueMessageConsumer<ApplyRegistryUpdateConsumer, ApplyRegistryUpdateMessage>();
builder.Services.AddQueueMessageConsumer<GetEntriesFromOrgConsumer, GetEntriesFromOrgMessage>();
builder.Services.AddQueueMessageConsumer<PostPeerConsumer, PostPeerMessage>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Scoped Service
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IPeerService, PeerService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<IPipelineService, PipelineService>();


// Add Scoped ResourceRegistry
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IRepositoryRepository, RepositoryRepository>();
builder.Services.AddScoped<IResourceTypeRepository, ResourceTypeRepository>();
builder.Services.AddScoped<IPeerRepository, PeerRepository>();
builder.Services.AddScoped<IPipelineRepository, PipelineRepository>();



builder.Services.AddDbContext<ResourceRegistryDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); }
);


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


