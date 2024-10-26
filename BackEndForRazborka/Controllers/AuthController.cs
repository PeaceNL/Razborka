using Microsoft.AspNetCore.Mvc;
using BackEndForRazborka.Models;
using BackEndForRazborka.DTO;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;


namespace BackEndForRazborka.Controllers
{

    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly ApplicationDbContext _context; 

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] UserDTO userDto)
        {
            try
            {
                Console.WriteLine("1");
                
                var condidat = await _context.Users.FirstOrDefaultAsync(x => x.Email == userDto.Email);
                
                Console.WriteLine("2");
                if (condidat == null)
                {
                    return Unauthorized("Такого пользователя не существует");
                }
                bool IsPaswordValid = BCrypt.Net.BCrypt.Verify(userDto.Password, condidat.Password);
                Console.WriteLine("3");
                if (!IsPaswordValid)
                {
                    return Unauthorized("Неверный пароль");
                }
                Console.WriteLine("4");
                var token = GenerateJwtToken(condidat);
                Console.WriteLine("5");
                return Ok(token);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Ошибка какая-то {ex.Message}");
            }
            
        }

        private static string GenerateJwtToken(User user) 
        {
            Console.WriteLine("1,1");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{Environment.GetEnvironmentVariable("JWT_SECRET_KEY")}"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Console.WriteLine("1,2");
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            Console.WriteLine("1,3");
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);
            Console.WriteLine("1,4");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
