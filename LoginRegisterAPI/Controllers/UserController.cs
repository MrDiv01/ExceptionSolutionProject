using LoginRegisterAPI.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UserController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> UsersList()
        {
            // Veritabanından kullanıcıları çek
            var data = await _context.Users.ToListAsync();

            // Eğer veritabanında kullanıcı yoksa 404 döndür
            if (data == null || data.Count == 0)
            {
                return NotFound("No users found.");
            }

            // Kullanıcıları UserResponse modeline dönüştür
            List<UserResponse> users = data.Select(user => new UserResponse
            {
                Name = user.FullName,
                Phones = user.PhoneNumber,
                UserId = user.Id,
                UserName = user.UserName
            }).ToList();

            // Başarılı bir şekilde kullanıcıları döndür
            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            // Kullanıcıyı ID'ye göre bul
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            // Kullanıcı bulunamazsa 404 döndür
            if (user == null)
            {
                return NotFound();
            }

            // Kullanıcıyı döndür
            return Ok(user);
        }
    }
}
