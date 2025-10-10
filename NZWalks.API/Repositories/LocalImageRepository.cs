using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : Repository<Image, Guid>, IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly NZWalksDbContext _nZWalksDbContext;
        public LocalImageRepository(NZWalksDbContext nZWalksDbContext,IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor): base(nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Image> UploadAsync(Image image)
        {
            
            // Get local path to store the image
            var localPathFile = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", image.FileName+image.FileExtension);

            // Upload the image to local storage
            FileStream stream = new FileStream(localPathFile, FileMode.Create);
            await image.File.CopyToAsync(stream);
            stream.Close();
            
            // Get relative path to access the image
            image.FilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                             $"{_httpContextAccessor.HttpContext.Request.Host.ToString()}" +
                             $"/Images/{image.FileName}{image.FileExtension}";
            
            _nZWalksDbContext.Images.Add(image);
            await _nZWalksDbContext.SaveChangesAsync();
            return image;
        }

        
    }
}
