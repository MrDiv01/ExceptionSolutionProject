using Microsoft.AspNetCore.Mvc;

namespace ExceptionSolutionProject.Controllers
{
    public class LoginRegisterController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return View();
        }
    }
}
