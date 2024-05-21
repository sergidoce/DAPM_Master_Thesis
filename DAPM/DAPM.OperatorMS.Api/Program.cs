using DAPM.OperatorMS.Api.Services.Interfaces;
using DAPM.OperatorMS.Api.Services;
using Microsoft.AspNetCore.Http.Features;
using Docker.DotNet;

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

//Docker Daemon
builder.Services.AddSingleton<DockerClient>(_ =>
{
    DockerClient client = new DockerClientConfiguration(
    new Uri("unix:///var/run/docker.sock"))
     .CreateClient();
    return client;
});

// Add services to the container.
builder.Services.AddScoped<IOperatorService, OperatorService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
