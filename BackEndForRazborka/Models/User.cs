
namespace BackEndForRazborka.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }    
}
