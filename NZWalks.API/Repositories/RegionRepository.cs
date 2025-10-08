using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository:Repository<Region, Guid>, IRegionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        public RegionRepository(NZWalksDbContext nZWalksDbContext):base(nZWalksDbContext)
        {
            this._nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Region?> UpdateAsync(Region entityRegion)
        {
            var existingRegion = await _nZWalksDbContext.Regions.FindAsync(entityRegion.Id);
            if (existingRegion == null)
            {
                return null;
            }
            _nZWalksDbContext.Regions.Update(entityRegion);
            await _nZWalksDbContext.SaveChangesAsync();
            return entityRegion;
        }
    }
}
