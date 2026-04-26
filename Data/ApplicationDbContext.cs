using AutiBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace AutiBuddy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
    }
}
