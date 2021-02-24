using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringApp.Data.Models;

namespace TutoringAppTests.Setup.Seeds
{
    public static class ModuleSeeder
    {
        public static async Task Seed(DbSet<Module> moduleSet)
        {
            var modules = new List<Module>
            {
                new Module // 1
                {
                    Name = "Operating Systems"
                },
                new Module // 2
                {
                    Name = "Databases"
                },
                new Module // 3
                {
                    Name = "Analysis of Algorithms"
                },
                new Module // 4
                {
                    Name = "Cyber Security"
                }
            };

            await moduleSet.AddRangeAsync(modules);
        }
    }
}
