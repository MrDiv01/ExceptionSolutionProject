using Microsoft.AspNetCore.Mvc;

namespace ExceptionSolutionProject.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
