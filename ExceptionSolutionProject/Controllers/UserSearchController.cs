using Microsoft.AspNetCore.Mvc;

namespace ExceptionSolutionProject.Controllers
{
    public class UserSearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
