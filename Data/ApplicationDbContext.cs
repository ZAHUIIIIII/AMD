using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UrlMapping> UrlMappings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}