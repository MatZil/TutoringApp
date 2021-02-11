using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TutoringApp.Data;

namespace TutoringApp.Configurations
{
    public static class StartupExtensions
    {
        public static void SetUpDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));

            services
                .BuildServiceProvider()
                .GetService<ApplicationDbContext>()?
                .Database.Migrate();
        }

        public static void StartAngularProject(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
