using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class AssignmentsRepository : BaseRepository<Assignment>
    {
        public AssignmentsRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.Assignments;
        }
    }
}
