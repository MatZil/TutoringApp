using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Models.Base;

namespace TutoringApp.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        protected DbSet<TEntity> ItemSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> filter)
        {
            return await ItemSet.AnyAsync(filter);
        }

        public virtual async Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = ItemSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            var items = await query.ToListAsync();

            return items;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var items = await ItemSet.ToArrayAsync();

            return items;
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            var items = await ItemSet.FirstOrDefaultAsync(obj => obj.Id.Equals(id));

            return items;
        }

        public virtual async Task<int> Create(TEntity entity)
        {
            await ItemSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            ItemSet.Update(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            ItemSet.Remove(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public virtual async Task CreateMany(IEnumerable<TEntity> entities)
        {
            await ItemSet.AddRangeAsync(entities);

            await _context.SaveChangesAsync();
        }
    }
}
