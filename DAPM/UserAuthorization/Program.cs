using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using UserAuthorization.Common;
using UserAuthorization.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configure Redis

// Configure Redis, similar to configuring a database connection string
var section = builder.Configuration.GetSection("Redis:Default");
// Connection string
string _connectionString = section.GetSection("Connection").Value;
// Instance name
string _instanceName = section.GetSection("InstanceName").Value;
// Default database 
int _defaultDB = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
builder.Services.AddSingleton(new RedisHelper(_connectionString, _instanceName, _defaultDB));

#endregion

// Configure interceptor, intercept all interfaces (cancel this module interceptor, put it in the gateway for unified interception)
////builder.Services.AddMvc(options =>
////{
////    // FactoryFillter interceptor class name
////    options.Filters.Add<FactoryFillter>();
////});

// Handle garbled data interaction between front-end and back-end
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

// Configure CORS
IConfiguration configuration = new ConfigurationBuilder()
.AddJsonFile("appsettings.json")
.Build();
builder.Services.AddCors(cor =>
{
    var cors = configuration.GetSection("CorsUrls").GetChildren().Select(p => p.Value);
    cor.AddPolicy("Cors", policy =>
    {
        policy.WithOrigins(cors.ToArray()) // Set allowed request origins
        .WithExposedHeaders("x-custom-header") // Set exposed response headers
        .AllowAnyHeader() // Allow all request headers
        .AllowAnyMethod() // Allow any method
        .AllowCredentials() // Allow cross-origin credentials - server must allow credentials
        .SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseCors("Cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
