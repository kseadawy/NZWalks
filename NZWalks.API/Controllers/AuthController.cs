using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository; 

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }
        // POST: api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequestDto registerRequestDto)
        {
            //Create a new user in the IdentityUser table
            IdentityUser identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };
            var identityResult = await _userManager.CreateAsync(identityUser,registerRequestDto.Password);
            if (identityResult.Succeeded && registerRequestDto.Roles!=null && registerRequestDto.Roles.Any())
            {
                identityResult = await _userManager.AddToRolesAsync(
                    identityUser,registerRequestDto.Roles);
                if (identityResult.Succeeded)
                {
                    return Ok("Register Succeeded! Please Login.");
                }
            }

            return BadRequest(identityResult);
        }
        // POST: api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequestDto)
        {
            var user =await _userManager.FindByEmailAsync(loginRequestDto.UserName);
            //Check user 
            if (user != null)
            {
                var passCheck = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (passCheck)
                {
                    //Get user roles
                    var roles = await _userManager.GetRolesAsync(user);

                    //Create JWT Token
                    var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                    return Ok(new LoginResponseDto
                    {
                        JwtToken = jwtToken
                    });
                }
            }
            return Unauthorized("User or Password is wrong. Please try again.");
        }
    }
}
