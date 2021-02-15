using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Configurations
{
    public static class StartupExtensions
    {
        #region ConfigureServices extensions
        public static void SetupDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));

            services
                .BuildServiceProvider()
                .GetService<ApplicationDbContext>()?
                .Database.Migrate();
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        public static void SetupWebToken(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var settings = configuration.GetSection("WebTokenSettings");
                    var securityKey = settings.GetSection("SecurityKey").Value;
                    var securityKeyBytes = Encoding.UTF8.GetBytes(securityKey);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = settings.GetSection("ValidIssuer").Value,
                        ValidAudience = settings.GetSection("ValidAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(securityKeyBytes)
                    };
                });
        }
        #endregion

        #region Configure extensions
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
        #endregion
    }
}
