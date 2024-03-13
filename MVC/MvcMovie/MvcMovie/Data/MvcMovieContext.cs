using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class MvcMovieContext(DbContextOptions<MvcMovieContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movie { get; set; } = default!;
    }
}
