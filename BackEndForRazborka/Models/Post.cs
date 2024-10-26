namespace BackEndForRazborka.Models
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } 
        public required string Title { get; set; }
        public string? Description { get; set; }
                
        public List<string> PhotoUrls { get; set; } = new List<string>();

        public required int User_id { get; set; }
        public User? User { get; set; }
    }
}
