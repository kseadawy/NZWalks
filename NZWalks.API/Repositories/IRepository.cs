using System.Linq.Expressions;

namespace NZWalks.API.Repositories
{
    public interface IRepository<T,U> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(U id,params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        
        Task<T?> DeleteAsync(U id);
    }
}
