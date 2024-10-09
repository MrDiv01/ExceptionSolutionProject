using ExceptionSolutionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ExceptionSolutionProject.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<AIChat> AIChats { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }
    }
}
