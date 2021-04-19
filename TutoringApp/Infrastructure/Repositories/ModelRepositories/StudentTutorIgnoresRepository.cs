using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public override async Task<IEnumerable<StudentTutorIgnore>> GetFiltered(Expression<Func<StudentTutorIgnore, bool>> filter = null)
        {
            IQueryable<StudentTutorIgnore> queryable = ItemSet
                .Include(i => i.Student);

            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            return await queryable.ToListAsync();
        }
    }
}
