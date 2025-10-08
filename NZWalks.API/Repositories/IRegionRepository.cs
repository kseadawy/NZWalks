using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository:IRepository<Region, Guid>
    {
        Task<Region?> UpdateAsync(Region entityRegion);
    }
}
