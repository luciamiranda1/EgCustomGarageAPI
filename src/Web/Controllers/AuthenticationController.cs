using System.Text;
using Application.Services;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] CredentialsRequest credentials)
        {
            var userLogged = await _userService.CheckCredentialsAsync(credentials);
            if (userLogged is null) return Unauthorized();

            var secret = _configuration["Authentication:SecretForKey"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userLogged.Id.ToString()),
                new Claim(ClaimTypes.Role, userLogged.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userLogged.Name ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, userLogged.Email ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(tokenStr);
        }
    }
}
