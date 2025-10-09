using NZWalks.API.Models.Domain;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NZWalks.API.Data;

namespace NZWalks.API.Repositories
{
    public class Repository<T,U> : IRepository<T, U> where T : class
    {
        private readonly NZWalksDbContext _nzWalksDbContext;
        private readonly DbSet<T> _dbSet;
        public Repository(NZWalksDbContext nzWalksDbContext)
        {
            _nzWalksDbContext = nzWalksDbContext;
            _dbSet = _nzWalksDbContext.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
             await _dbSet.AddAsync(entity);
             await _nzWalksDbContext.SaveChangesAsync();
             return entity;

        }

        public async Task<T?> DeleteAsync(U id)
        {
            var removedEntity = await _dbSet.FindAsync(id);
            if (removedEntity == null)
            {
                return null;
            }
            _dbSet.Remove(removedEntity);
            await _nzWalksDbContext.SaveChangesAsync();
            return removedEntity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filterExpression=null, 
            string? sortBy=null, bool ascending = true,
            int pageNumber = 1, int pageSize = 100,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            // Apply eager loading for related entities
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            //Filtering
            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            // Generic sorting by property name using reflection and Expression
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.PropertyOrField(parameter, sortBy);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = ascending? "OrderBy" : "OrderByDescending";
                var resultExp = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new Type[] { typeof(T), property.Type },
                    query.Expression,
                    Expression.Quote(lambda));

                query = query.Provider.CreateQuery<T>(resultExp);
            }
            // Pagination
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 100;
            }
            var skip = (pageNumber - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
            return query.AsEnumerable();
        }

        public async Task<T?> GetByIdAsync(U id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var returnedEntity = await _dbSet.FindAsync(id);
            return returnedEntity;
        }
    }
}
