namespace NZWalks.API.Repositories
{
    public interface IRepository<T,U> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(U id);
        Task<T> AddAsync(T entity);
        
        Task<T?> DeleteAsync(U id);
    }
}
