using NuGet.Protocol.Core.Types;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IImageRepository: IRepository<Image, Guid>
    {
        Task<Image> UploadAsync(Image image);
    }
}
