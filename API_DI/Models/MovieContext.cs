using Microsoft.EntityFrameworkCore;

namespace API_DI.Models
{
    public class MovieContext: DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options) 
        {

        }
        public DbSet<Movie> Movie { get; set; } = null;
    }
}
