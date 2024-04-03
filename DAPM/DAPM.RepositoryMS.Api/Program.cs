using DAPM.RepositoryMS.Api.Models;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Repositories;
using DAPM.RepositoryMS.Api.Services;
using Microsoft.AspNetCore.Http.Features;
using DAPM.RepositoryMS.Api.Services.Interfaces;

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Services
builder.Services.AddScoped<IResourceService, ResourceService>();


//Repositories
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();

builder.Services.Configure<ResourceDatabaseSettings>(
    builder.Configuration.GetSection("ResourceDatabase"));

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
