using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        // POST: api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadAsync([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
            if (imageUploadRequestDto.File == null || imageUploadRequestDto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            ValidateFileUpload(imageUploadRequestDto);
            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileName = imageUploadRequestDto.FileName,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                    Description = imageUploadRequestDto.FileDescription

                };
                var uploadedImage = await _imageRepository.UploadAsync(imageDomainModel);
                return Ok(uploadedImage);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            var permittedExtensions = new String[] { ".jpg", ".jpeg", ".png"};
            var ext = Path.GetExtension(imageUploadRequestDto.File.FileName).ToLowerInvariant();
            // Check the file extension against the list of permitted extensions
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError("extension", "Unsupported file extension");
            }

            // Check the file size (5MB maximum)
            
            long fileSizeInBytes = imageUploadRequestDto.File.Length;
            long maxFileSizeInBytes = 5 * 1024 * 1024; // 5MB
            if (fileSizeInBytes > maxFileSizeInBytes)
            {
                ModelState.AddModelError("fileSize", "File size exceeds the 5MB limit.");
            }    

        }
    }
}
