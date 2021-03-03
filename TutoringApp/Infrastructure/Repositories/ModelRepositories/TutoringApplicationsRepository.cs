using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class TutoringApplicationsRepository : BaseRepository<TutoringApplication>
    {
        public TutoringApplicationsRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.TutoringApplications;
        }
    }
}
