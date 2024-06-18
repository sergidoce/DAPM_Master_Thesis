using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using DAPM.PeerApi.Consumers;
using RabbitMQLibrary.Messages.PeerApi;
using DAPM.PeerApi.Services.Interfaces;
using DAPM.PeerApi.Services;
using RabbitMQLibrary.Messages.PeerApi.Handshake;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddQueueing(new QueueingConfigurationSettings
{
    RabbitMqConsumerConcurrency = 5,
    RabbitMqHostname = "rabbitmq",
    RabbitMqPort = 5672,
    RabbitMqPassword = "guest",
    RabbitMqUsername = "guest"
});

builder.Services.AddQueueMessageConsumer<SendHandshakeAckConsumer, SendHandshakeAckMessage>();
builder.Services.AddQueueMessageConsumer<SendHandshakeRequestConsumer, SendHandshakeRequestMessage>();
builder.Services.AddQueueMessageConsumer<SendHandshakeRequestResponseConsumer, SendHandshakeRequestResponseMessage>();
builder.Services.AddQueueMessageConsumer<SendRegistryUpdateConsumer, SendRegistryUpdateMessage>();


builder.Services.AddScoped<IActionService, ActionService>();
builder.Services.AddScoped<IHandshakeService, HandshakeService>();
builder.Services.AddScoped<IHttpService, HttpService>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

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
