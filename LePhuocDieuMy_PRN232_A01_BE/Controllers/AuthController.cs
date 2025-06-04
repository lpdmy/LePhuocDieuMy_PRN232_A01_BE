using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LePhuocDieuMy_PRN232_A01_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ISystemAccountRepository _accountRepo;
        private readonly IConfiguration _config;
        public AuthController(ISystemAccountRepository accountRepo, IConfiguration config)
        {
            _accountRepo = accountRepo;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var adminEmail = _config["AdminAccount:Email"];
            var adminPassword = _config["AdminAccount:Password"];

            if (model.Email == adminEmail && model.Password == adminPassword)
            {
                // Create Admin JWT token
                var jwtSettingsAdmin = _config.GetSection("Jwt");
                var keyAdmin = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsAdmin["Key"]));

                var claimsAdmin = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "admin"), // Or any unique id for admin
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var tokenAdmin = new JwtSecurityToken(
                    issuer: jwtSettingsAdmin["Issuer"],
                    audience: jwtSettingsAdmin["Audience"],
                    claims: claimsAdmin,
                    expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettingsAdmin["ExpireMinutes"])),
                    signingCredentials: new SigningCredentials(keyAdmin, SecurityAlgorithms.HmacSha256)
                );

                var tokenStringAdmin = new JwtSecurityTokenHandler().WriteToken(tokenAdmin);
                return Ok(new { Token = tokenStringAdmin });
            }

            var user = _accountRepo.GetAll()
                .FirstOrDefault(u => u.AccountEmail == model.Email && u.AccountPassword == model.Password);

            if (user == null)
                return Unauthorized();

            string roleName = user.AccountRole switch
            {
                1 => "Staff",
                2 => "Lecturer ",
                _ => "Admin"  
            };

            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Name, user.AccountName),
                new Claim(ClaimTypes.Role, roleName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpireMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token = tokenString });
        }
    }
}
