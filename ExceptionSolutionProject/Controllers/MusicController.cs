using Microsoft.AspNetCore.Mvc;

namespace ExceptionSolutionProject.Controllers
{
    public class MusicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
