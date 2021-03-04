using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class ModulesRepository : BaseRepository<Module>
    {
        public ModulesRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.Modules;
        }

        public override async Task<Module> GetById(int id)
        {
            return await ItemSet
                    .Include(m => m.ModuleTutors)
                    .Include(m => m.TutoringApplications)
                    .FirstOrDefaultAsync(m => m.Id == id)
                ;
        }
    }
}
