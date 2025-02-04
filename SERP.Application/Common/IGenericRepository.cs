using SERP.Domain.Common;
using System.Linq.Expressions;

namespace SERP.Application.Common
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<T?> GetByIdAsync(Expression<Func<T, bool>> condition);
        Task CreateAsync(T entity);
        Task CreateRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetQueryPaged(int page, int pageSize, Expression<Func<T, bool>> predicate);
        Task<int> CountQueryPaged(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetQuery();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindMultiple(Expression<Func<T, bool>> expression);
        Task<Dictionary<int, T>> GetDictionaryAsync(Expression<Func<T, bool>> condition);    }
}
