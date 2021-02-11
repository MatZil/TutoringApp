using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    }
}
