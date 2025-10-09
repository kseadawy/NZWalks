using System.Linq.Expressions;

namespace NZWalks.API.Repositories
{
    public interface IRepository<T,U> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filterExpression=null, 
            string? sortBy=null, bool ascending = true,
            int pageNumber= 1, int pageSize = 100,
            params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(U id,params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        
        Task<T?> DeleteAsync(U id);
    }
}
