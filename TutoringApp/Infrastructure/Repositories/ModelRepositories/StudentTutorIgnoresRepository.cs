using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Models.JoiningTables;
using TutoringApp.Infrastructure.Repositories.Interfaces;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class StudentTutorIgnoresRepository : IStudentTutorIgnoresRepository
    {
        private readonly DbSet<StudentTutorIgnore> _ignoresSet;

        public StudentTutorIgnoresRepository(ApplicationDbContext context)
        {
            _ignoresSet = context.StudentTutorIgnores;
        }

        public async Task<bool> TutorIgnoresStudent(string tutorId, string studentId)
        {
            return await _ignoresSet.AnyAsync(i => i.StudentId == studentId && i.TutorId == tutorId);
        }
    }
}
