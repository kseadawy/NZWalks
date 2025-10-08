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

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return  query.AsEnumerable();
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
