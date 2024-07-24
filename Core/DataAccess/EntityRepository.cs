using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public class EntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
          where TEntity : BaseEntity, new()
          where TContext : DbContext, new()
    {
        public int? Add(TEntity entity)
        {
            using (TContext context = new TContext())//using içindeki context garbage collector yardımıyla belleği hızlıca temizler.Performans için yazdık.
            {
                var addedEntity = context.Entry(entity);
                entity.CreateDate = DateTime.Now;
                addedEntity.State = EntityState.Added;//Ekleme işlemi yapılacağını bildirdik. 

                context.SaveChanges();//İşlemleri gerçekleştir.

                /// GetId
                return entity.Id;
            }
        }
        public bool Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                entity.UpdateDate = DateTime.Now;
                updatedEntity.State = EntityState.Modified;

                return context.SaveChanges() > 1 ? true : false;
            }
        }

        public bool Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);

                deletedEntity.State = EntityState.Deleted;

                return context.SaveChanges() > 1 ? true : false;
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
