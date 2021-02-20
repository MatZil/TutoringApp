using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class TutorEvaluationsRepository : BaseRepository<TutorEvaluation>
    {
        public TutorEvaluationsRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.TutorEvaluations;
        }
    }
}
