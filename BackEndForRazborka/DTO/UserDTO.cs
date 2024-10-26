using System.ComponentModel.DataAnnotations;

namespace BackEndForRazborka.DTO
{
    public class UserDTO
    {
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public required string Email { get; set; }

        [StringLength(32, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        public required string Password { get; set; }
        
    }
}
