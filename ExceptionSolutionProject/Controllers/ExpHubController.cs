    using ExceptionSolutionProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExceptionSolutionProject.Controllers
{
    public class ExpHubController : Controller
    {
        private readonly ExpHubApplicationDbContext _expContext;

        
        public ExpHubController(ExpHubApplicationDbContext expContext)
        {
            _expContext = expContext;
        }
        public async Task<IActionResult> Index()
        
        {
            var data = await _expContext.Folders.ToListAsync();
            return View(data);
        }
    }
}
