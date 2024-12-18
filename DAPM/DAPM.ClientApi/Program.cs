using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using RabbitMQ.Client;
using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using DAPM.ClientApi.Consumers;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using Microsoft.OpenApi.Models;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UtilLibrary.Services;
using UtilLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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


// RabbitMQ
builder.Services.AddQueueing(new QueueingConfigurationSettings
{
    RabbitMqConsumerConcurrency = 5,
    RabbitMqHostname = "rabbitmq",
    RabbitMqPort = 5672,
    RabbitMqPassword = "guest",
    RabbitMqUsername = "guest"
});

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DAPM Client API", Version = "v1" });
});

//AUTHENTICATOR
builder.Services.AddQueueMessageConsumer<RegisterUserResultConsumer, RegisterUserResultMessage>();
builder.Services.AddQueueMessageConsumer<LoginResultConsumer, LoginResultMessage>();
builder.Services.AddQueueMessageConsumer<AddRolesResultConsumer, AddRolesResultMessage>();
builder.Services.AddQueueMessageConsumer<DeleteUserResultConsumer, DeleteUserResultMessage>();
builder.Services.AddQueueMessageConsumer<EditAsAdminResultConsumer, EditAsAdminResultMessage>();
builder.Services.AddQueueMessageConsumer<EditAsUserResultConsumer, EditAsUserResultMessage>();
builder.Services.AddQueueMessageConsumer<GetRolesResultConsumer, GetRolesResultMessage>();
builder.Services.AddQueueMessageConsumer<GetUsersResultConsumer, GetUsersResultMessage>();
builder.Services.AddQueueMessageConsumer<SetOrganizationResultConsumer, SetOrganizationResultMessage>();
builder.Services.AddQueueMessageConsumer<SetRolesResultConsumer, SetRolesResultMessage>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();

builder.Services.AddQueueMessageConsumer<GetOrganizationsProcessResultConsumer, GetOrganizationsProcessResult>();
builder.Services.AddQueueMessageConsumer<PostItemResultConsumer, PostItemProcessResult>();
builder.Services.AddQueueMessageConsumer<GetRepositoriesProcessResultConsumer, GetRepositoriesProcessResult>();
builder.Services.AddQueueMessageConsumer<GetResourcesProcessResultConsumer, GetResourcesProcessResult>();
builder.Services.AddQueueMessageConsumer<GetPipelinesProcessResultConsumer, GetPipelinesProcessResult>();
builder.Services.AddQueueMessageConsumer<GetResourceFilesProcessResultConsumer, GetResourceFilesProcessResult>();
builder.Services.AddQueueMessageConsumer<CollabHandshakeProcessResultConsumer, CollabHandshakeProcessResult>();
builder.Services.AddQueueMessageConsumer<PostPipelineCommandProcessResultConsumer, PostPipelineCommandProcessResult>();
builder.Services.AddQueueMessageConsumer<GetPipelineExecutionStatusProcessResultConsumer, GetPipelineExecutionStatusRequestResult>();

builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<IPipelineService, PipelineService>();
builder.Services.AddSingleton<ITicketService, TicketService>();
builder.Services.AddScoped<ISystemService, SystemService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IAuthenticatorService, AuthenticatorService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton(new FileLogService(Path.Combine(AppContext.BaseDirectory, "Logs", "ActivityLog.txt")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration.GetSection("JWTTokenKey").Value;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(builder.Configuration.GetSection("JWTTokenKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    // Add the security definition for Bearer token (JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add the security requirement to include Bearer token globally
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
});

var app = builder.Build();

app.MapDefaultEndpoints();



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

    // Optionally: Automatically add "Bearer" prefix in the input field
    c.RoutePrefix = "swagger"; // Set Swagger UI at the root
});

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ActivityLogMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
