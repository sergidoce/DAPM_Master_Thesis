
using AutoMapper;
using DAPM.Authenticator.Data;
using DAPM.Authenticator.Models;
using DAPM.Authenticator.Services;
using DAPM.Authenticator.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAPM.Authenticator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring)));

            builder.Services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;

            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddEntityFrameworkStores<DataContext>();

            builder.Services.AddScoped<TokenService>();


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            var services = app.Services.CreateScope().ServiceProvider;
            try {
                var datacontext = services.GetRequiredService<DataContext>();
                await datacontext.Database.MigrateAsync();
                var usermanager = services.GetRequiredService<UserManager<User>>();
                var rolemanager = services.GetRequiredService<RoleManager<Role>>();

                //create some beginner roles incase they dont exist
                List<string> listOfRoles = new List<string>() { "Standard", "Admin", "Privileged" };
                foreach (var role in listOfRoles)
                {
                    await rolemanager.CreateAsync(new Role { Name = role });
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Error during migration {ex}");
            }

            app.Run();
        }
    }
}
