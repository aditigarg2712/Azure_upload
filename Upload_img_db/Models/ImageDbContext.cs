using Microsoft.EntityFrameworkCore;

namespace Upload_img_db.Models
{
    public class ImageDbContext:DbContext
    {
        public ImageDbContext(DbContextOptions options) : base(options) 
        { 

        }
        public DbSet<ImageModel> Images { get; set; }   
    }
}
