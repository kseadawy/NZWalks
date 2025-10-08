using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(3, ErrorMessage = "length must not be more than 3 characters")]
        [MinLength(3, ErrorMessage = "length must not be less than 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "length must not be more than 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
