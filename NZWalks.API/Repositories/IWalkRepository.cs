using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository:IRepository<Walk, Guid>
    {
        Task<Walk?> UpdateAsync(Walk entityWalk);
    }
}
