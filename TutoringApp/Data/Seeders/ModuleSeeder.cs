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
                new Module { Id = 2, Name = "Analysis of Algorithms" },
                new Module { Id = 3, Name = "Databases" },
                new Module { Id = 4, Name = "Software Testing" },
                new Module { Id = 5, Name = "Object-Oriented Programming I" },
                new Module { Id = 6, Name = "Object-Oriented Programming II" },
                new Module { Id = 7, Name = "Mathematics I" }
            );
        }
    }
}
