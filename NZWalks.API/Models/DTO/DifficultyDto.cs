using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Domain
{
    public class DifficultyDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
