using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Models.JoiningTables;
using TutoringApp.Infrastructure.Repositories.Interfaces;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class ModuleTutorsRepository : IModuleTutorsRepository
    {
        private readonly DbSet<ModuleTutor> _moduleTutorSet;
        private readonly ApplicationDbContext _context;

        public ModuleTutorsRepository(ApplicationDbContext context)
        {
            _context = context;
            _moduleTutorSet = context.ModuleTutors;
        }

        public async Task Delete(int moduleId, string tutorId)
        {
            var moduleTutor = await _moduleTutorSet.FirstOrDefaultAsync(mt => mt.ModuleId == moduleId && mt.TutorId == tutorId);

            if (moduleTutor != null)
            {
                _moduleTutorSet.Remove(moduleTutor);

                await _context.SaveChangesAsync();
            }
        }
    }
}
