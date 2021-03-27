using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public override async Task<IEnumerable<TutoringSession>> GetFiltered(Expression<Func<TutoringSession, bool>> filter = null)
        {
            IQueryable<TutoringSession> query = ItemSet
                .Include(ts => ts.Module)
                .Include(ts => ts.Student)
                .Include(ts => ts.Tutor)
                ;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var items = await query.ToListAsync();

            return items;
        }

        public override async Task<TutoringSession> GetById(int id)
        {
            var items = await ItemSet
                .Include(ts => ts.Tutor)
                .Include(ts => ts.Student)
                .FirstOrDefaultAsync(obj => obj.Id.Equals(id));

            return items;
        }
    }
}
