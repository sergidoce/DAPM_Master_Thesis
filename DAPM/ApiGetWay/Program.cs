using ApiGetWay.Common;
using ApiGetWay.Controllers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

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

// Enable authentication middleware: note, this must be started first
app.UseAuthentication();
// Enable custom authorization middleware
app.UseAuthorizationWithCust();

// Enable authorization middleware
// app.UseAuthentication() enables authentication middleware, which verifies the identity information in the request and stores it in the HttpContext.User property.
// app.UseAuthorization() enables authorization middleware, which checks whether the identity information in HttpContext.User has the necessary permissions to access the current request.
// Make sure to enable authentication middleware before enabling authorization middleware, because the authorization middleware needs to use the identity information stored by the authentication middleware for permission verification.
// If the authentication middleware is not enabled, the authorization middleware will not be able to obtain identity information and therefore cannot perform authorization verification.
app.UseAuthorization();

app.MapControllers();

app.UseOcelot();

app.Run();
