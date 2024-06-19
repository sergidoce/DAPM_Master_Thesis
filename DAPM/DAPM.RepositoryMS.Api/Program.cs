using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Repositories;
using DAPM.RepositoryMS.Api.Services;
using Microsoft.AspNetCore.Http.Features;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Extensions;
using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Messages.ResourceRegistry;
using DAPM.RepositoryMS.Api.Consumers;
using RabbitMQLibrary.Messages.Repository;
using Microsoft.EntityFrameworkCore;
using DAPM.RepositoryMS.Api.Data;

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

builder.Services.AddQueueMessageConsumer<PostResourceToRepoConsumer, PostResourceToRepoMessage>();
builder.Services.AddQueueMessageConsumer<PostOperatorToRepoConsumer, PostOperatorToRepoMessage>();
builder.Services.AddQueueMessageConsumer<PostRepoToRepoConsumer, PostRepoToRepoMessage>();
builder.Services.AddQueueMessageConsumer<PostPipelineToRepoConsumer, PostPipelineToRepoMessage>();
builder.Services.AddQueueMessageConsumer<GetPipelinesFromRepoConsumer, GetPipelinesFromRepoMessage>();
builder.Services.AddQueueMessageConsumer<GetResourceFilesFromRepoConsumer, GetResourceFilesFromRepoMessage>();



builder.Services.AddDbContext<RepositoryDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
}
);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Services
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<IPipelineService, PipelineService>();
builder.Services.AddScoped<IFileService, FileService>();

//Repositories
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IRepositoryRepository, RepositoryRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IPipelineRepository, PipelineRepository>();
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();


builder.Services.Configure<FileStorageDatabaseSettings>(
    builder.Configuration.GetSection("FileStorageDatabase"));

var app = builder.Build();

app.MapDefaultEndpoints();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
