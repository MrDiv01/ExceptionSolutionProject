using ExceptionSolutionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ExceptionSolutionProject.Data
{
    public class ExpHubApplicationDbContext:DbContext
    {
        public ExpHubApplicationDbContext(DbContextOptions<ExpHubApplicationDbContext> options) :base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Folder ile User arasında One-to-Many ilişkiyi açıkça tanımla
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.User)
                .WithMany(u => u.Folders)
                .HasForeignKey(f => f.UserId);
            base.OnModelCreating(modelBuilder);
        }

    }
}
