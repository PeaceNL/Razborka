using Microsoft.EntityFrameworkCore;

namespace BackEndForRazborka.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; } = null!;        
        public DbSet<Post> Posts { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь User -> Post : 1 -> *
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts) //У одного пользователя много постов
                .WithOne(p => p.User) //У поста может быть один пользователь
                .HasForeignKey(p => p.User_id)  //Сваязаны по ключу User.id
                .OnDelete(DeleteBehavior.Cascade); // При удалении юзера все посты удаляются автоматически 
        }
    }    
}
