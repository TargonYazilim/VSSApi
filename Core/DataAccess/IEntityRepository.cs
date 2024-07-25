using Core.Entities;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> where T : BaseEntity, new()
    {
        Task<List<T>?> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool tracking = false, params Expression<Func<T, object>>[] includes);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracking = false, params Expression<Func<T, object>>[] includes);
        Task<int?> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
