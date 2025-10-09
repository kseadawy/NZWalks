using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MaxLength(3, ErrorMessage = "length must be 3 characters")]
        [MinLength(3, ErrorMessage = "length must be 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "length must not be more than 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
