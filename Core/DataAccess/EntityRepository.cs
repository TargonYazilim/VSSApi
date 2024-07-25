using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public class EntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
          where TEntity : BaseEntity, new()
          where TContext : DbContext, new()
    {
        public async Task<int?> Add(TEntity entity)
        {
            using (TContext context = new TContext())//using içindeki context garbage collector yardımıyla belleği hızlıca temizler.Performans için yazdık.
            {

                EntityEntry<TEntity> entityEntry = context.Add(entity);
                entityEntry.State = EntityState.Added;
                await context.SaveChangesAsync();//İşlemleri gerçekleştir.

                /// GetId
                return entity.Id;
            }
        }
        public async Task<bool> Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                EntityEntry<TEntity> entityEntry = context.Update(entity);
                entityEntry.State = EntityState.Modified;

                return (await context.SaveChangesAsync()) > 1 ? true : false;
            }
        }

        public async Task<bool> Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);

                deletedEntity.State = EntityState.Deleted;

                return await context.SaveChangesAsync() > 1 ? true : false;
            }
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, bool tracking = false, params Expression<Func<TEntity, object>>[] includes)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (!tracking)
                    query = query.AsNoTracking();

                if (includes != null)
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                return await query.SingleOrDefaultAsync(filter);
            }
        }

        public async Task<List<TEntity>?> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, bool tracking = false, params Expression<Func<TEntity, object>>[] includes)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();
                if (!tracking)
                    query = query.AsNoTracking();

                if (includes != null)
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                if (filter != null)
                    query = query.Where(filter);

                return await query.ToListAsync();
            }
        }
    }
}
