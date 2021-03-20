using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TutoringApp.Configurations;
using TutoringApp.Configurations.Auth;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.SignalR.Hubs;

namespace TutoringApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.SetupDatabase(Configuration);
            services.AddIdentity();
            services.SetupWebToken(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.ConfigureDependencyInjections();
            services.AddSignalR(o => o.EnableDetailedErrors = true);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MainHub>("api/hubs/main", options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                });
            });

            app.StartAngularProject(env);

            InitialAdminSeeder.SeedInitialAdmin(Configuration, userManager);
        }
    }
}
