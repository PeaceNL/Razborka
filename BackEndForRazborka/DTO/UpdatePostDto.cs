using System.ComponentModel.DataAnnotations;

namespace BackEndForRazborka.DTO
{
    public class UpdatePostDto
    {
        [Required(ErrorMessage = "Название поста обязательно.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Описание поста обязательно.")]
        public required string Description { get; set; }

        public required List<string> PhotoUrls { get; set; }
    }
}
