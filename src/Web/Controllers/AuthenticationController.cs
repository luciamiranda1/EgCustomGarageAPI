using System.Text;
using Application.Services;
using Application.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Web.Controllers //indica donde esta ubicado el controlador
{
    [Route("api/[controller]")] //define la ruta base
    [ApiController] //requiere rutas por atributos .
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;//dependencias
        private readonly IConfiguration _configuration; //dependencias

        public AuthenticationController(UserService userService, IConfiguration configuration) //constructor, donde ocurre la inyeccion de dep.
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
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
