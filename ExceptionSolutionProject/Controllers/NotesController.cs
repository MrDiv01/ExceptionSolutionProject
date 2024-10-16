using Microsoft.AspNetCore.Mvc;

namespace ExceptionSolutionProject.Controllers
{
    public class NotesController : Controller
    {
        //TO DO : HangFire Elave olunacaq Istifadeci Mailini daxil etdikden sonra
        //Zamani geldikde o maile Bildiris gedecek Iclas barede melumat verilecek.
        public IActionResult Index()
        {
            return View();
        }
    }
}
