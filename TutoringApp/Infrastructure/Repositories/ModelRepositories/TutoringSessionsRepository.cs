using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class TutoringSessionsRepository : BaseRepository<TutoringSession>
    {
        public TutoringSessionsRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.TutoringSessions;
        }
    }
}
