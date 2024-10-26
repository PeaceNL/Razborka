using BackEndForRazborka.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEndForRazborka.DTO;


namespace BackEndForRazborka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet()]
        public IActionResult Get()
        {
           
            return Ok("Ты на начальной странице и всё работает хотябы");
        }
          
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var condidate = await _context.Users.FirstOrDefaultAsync(u => u.Email == entity.Email);
            if (condidate != null)
            {
                return BadRequest(new { message = "Такой пользователь уже сущетсвует!" });
            }
            var user = new User
            {
                Email = entity.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(entity.Password)
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }  
    }    
}
