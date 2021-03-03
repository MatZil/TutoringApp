using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public override async Task<IEnumerable<TutoringApplication>> GetAll()
        {
            return await ItemSet
                .Include(ta => ta.Module)
                .Include(ta => ta.Student)
                .ToListAsync();
        }
    }
}
