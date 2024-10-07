using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; } // Kullanıcının tam adı
    public string? PhoneNumber { get; set; } // Kullanıcının telefon numarası
}
