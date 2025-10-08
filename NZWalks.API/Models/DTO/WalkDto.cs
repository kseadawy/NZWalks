using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Models.Domain
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage = "Name must not be more than 100 characters")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }


        [ValidateNever]
        public DifficultyDto Difficulty { get; set; }

        [ValidateNever]
        public RegionDto Region { get; set; }
    }
}
