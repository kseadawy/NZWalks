using NZWalks.API.Models.Domain;
using System;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return  _dbSet.AsEnumerable();
        }

        public async Task<T?> GetByIdAsync(U id)
        {
            var returnedEntity = await _dbSet.FindAsync(id);
            if (returnedEntity == null)
            {
                return null;
            }
            
            return returnedEntity;
        }
    }
}
