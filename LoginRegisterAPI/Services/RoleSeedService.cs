using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class RoleSeedService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleSeedService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedRolesAsync()
    {
        // "Admin" rolünü kontrol et, yoksa oluştur
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // "User" rolünü kontrol et, yoksa oluştur
        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
    }
}
