using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApiDbContext : IdentityDbContext<AppUser>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; } // Eğer başka DbSet'leriniz varsa buraya ekleyebilirsiniz
}
