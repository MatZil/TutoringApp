using TutoringApp.Data;
using TutoringApp.Data.Models.JoiningTables;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class StudentTutorIgnoresRepository : BaseRepository<StudentTutorIgnore>
    {
        public StudentTutorIgnoresRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.StudentTutorIgnores;
        }
    }
}
