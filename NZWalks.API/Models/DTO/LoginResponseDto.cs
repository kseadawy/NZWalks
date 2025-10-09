using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Models.DTO
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; }
    }
}
