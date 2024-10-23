using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string Name {  get; set; }
    public string Surname {  get; set; }
    public string Specialty { get; set; }
    public string PhoneNumber { get; set; }
}
