using Microsoft.EntityFrameworkCore;
using TutoringApp.Data.Models;

namespace TutoringApp.Data.Seeders
{
    public static class ModuleSeeder
    {
        public static void SeedModules(this ModelBuilder builder)
        {
            builder.Entity<Module>().HasData(
                new Module { Id = 1, Name = "Cyber Security" },
                new Module { Id = 1, Name = "Analysis of Algorithms" },
                new Module { Id = 1, Name = "Databases" },
                new Module { Id = 1, Name = "Software Testing" },
                new Module { Id = 1, Name = "Object-Oriented Programming I" },
                new Module { Id = 1, Name = "Object-Oriented Programming II" },
                new Module { Id = 1, Name = "Mathematics I" }
            );
        }
    }
}
