using TutoringApp.Data;
using TutoringApp.Data.Models.JoiningTables;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class StudentTutorsRepository : BaseRepository<StudentTutor>
    {
        public StudentTutorsRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.StudentTutors;
        }
    }
}
