using TutoringApp.Data;
using TutoringApp.Data.Models;

namespace TutoringApp.Infrastructure.Repositories.ModelRepositories
{
    public class TutoringRequestsRepository : BaseRepository<TutoringRequest>
    {
        public TutoringRequestsRepository(ApplicationDbContext context) : base(context)
        {
            ItemSet = context.TutoringRequests;
        }
    }
}
