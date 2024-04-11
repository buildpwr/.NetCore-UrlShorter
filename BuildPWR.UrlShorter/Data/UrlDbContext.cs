using BuildPWR.UrlShorter.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildPWR.UrlShorter.Data
{
    public class UrlDbContext : DbContext
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
        {
        }

        public DbSet<UrlItem> UrlItems { get; set; }
    }

}
