using BackEndForRazborka.Models;
using System.ComponentModel.DataAnnotations;

namespace BackEndForRazborka.DTO
{
    public class PostDto
    {
        [Required(ErrorMessage = "Название поста обязательно.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Описание поста обязательно.")]
        public required string Description { get; set; }      

    }
}
