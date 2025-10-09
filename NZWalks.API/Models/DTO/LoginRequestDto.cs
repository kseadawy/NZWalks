using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Models.DTO
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }
    }
}
