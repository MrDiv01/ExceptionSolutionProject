    using ExceptionSolutionProject.Data;
using ExceptionSolutionProject.Helper;
using ExceptionSolutionProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExceptionSolutionProject.Controllers
{
    [Authorize]
    public class ExpHubController : Controller
    {
        private readonly ExpHubApplicationDbContext _expContext;

        
        public ExpHubController(ExpHubApplicationDbContext expContext)
        {
            _expContext = expContext;
        }
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
                var userEmail = AiHelper.GetEmailFromJwt(token);

            User data = await _expContext.Users.Where(x => x.Email == userEmail).Include(x => x.Folders).FirstOrDefaultAsync();
                
                //var data = await _expContext.Folders.ToListAsync();
            return View(data);
        }
    }
}
