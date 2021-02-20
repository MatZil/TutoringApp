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
    }
}
