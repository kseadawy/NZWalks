using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository:Repository<Walk, Guid>, IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        public WalkRepository(NZWalksDbContext nZWalksDbContext):base(nZWalksDbContext)
        {
            this._nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Walk?> UpdateAsync(Walk entityWalk)
        {
            var existingWalk = await _nZWalksDbContext.Walks.FindAsync(entityWalk.Id);
            if (existingWalk == null)
            {
                return null;
            }
            _nZWalksDbContext.Walks.Update(entityWalk);
            await _nZWalksDbContext.SaveChangesAsync();
            return entityWalk;
        }
    }
}
