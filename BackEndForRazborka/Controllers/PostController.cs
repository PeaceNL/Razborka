using BackEndForRazborka.Models;
using BackEndForRazborka.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEndForRazborka.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BackEndForRazborka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly S3Service S3Service;
        private readonly ApplicationDbContext _context;
        
        public PostController(ApplicationDbContext context, S3Service s3Service)
        {
            _context = context;
            this.S3Service = s3Service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(string id)
        {
            try
            {
                var post = await _context.Posts.FindAsync(int.Parse(id));
                Console.WriteLine(post);
                if (post == null)
                {
                    return NotFound("Такого поста нет");
                }
                return Ok(post);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
           
        }

        [Authorize]
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreatePost(int userId, [FromForm] PostDto postDto, List<IFormFile> files)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (files == null || files.Count == 0)
            {
                return BadRequest("Необходимо загрузить хотя бы одно изображение.");
            }
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; ;
            if (currentUserIdClaim == null)
            {
                return Unauthorized("Авторизируйтесь!");
            }
            if (int.Parse(currentUserIdClaim) != userId)
            {
                return Forbid("У вас нет на это доступа");
            }

            try
            {
                // Создание нового поста
                var fileUrls = await S3Service.UploadFileAsync(files);

                var post = new Post
                {
                    Title = postDto.Title,
                    Description = postDto.Description,
                    User_id = userId,
                    PhotoUrls = new List<string>(fileUrls),
                    CreatedDate = DateTime.UtcNow
                };

                // Сохранение поста в базе данных
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Пост успешно создан", post });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при создании поста: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPost(int id, [FromBody] UpdatePostDto updatePost)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdClaim == null)
            {
                return Unauthorized("Не удалось получить информацию о пользователе.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Введите корректные данные");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Такого поста нет");
            }

            if (int.Parse(currentUserIdClaim) != post.User_id)
            {
                return Forbid("У вас нет на это доступа");
            }

            post.Description = updatePost.Description;
            post.Title = updatePost.Title;
            post.CreatedDate = DateTime.UtcNow;

            var missingElements = post.PhotoUrls.Except(updatePost.PhotoUrls).ToList();
            
            if (missingElements.Count > 0)
            {
                await S3Service.DeleteFileAsync(missingElements);
            }
            
            post.PhotoUrls = updatePost.PhotoUrls;

            await _context.SaveChangesAsync();

            return Ok($"запатчил POST {id}");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {            
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"Текущий id из токена : {currentUserIdClaim}");            
            if (currentUserIdClaim == null)
            {
                return Unauthorized("Не удалось получить информацию о пользователе.");
            }
            
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return BadRequest("Такого поста нет");
                }
                if (int.Parse(currentUserIdClaim) != post.User_id)
                {
                    return Forbid();
                }
                await S3Service.DeleteFileAsync(post.PhotoUrls);
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении поста: {ex.Message}");
            }            
        }
    }
}
