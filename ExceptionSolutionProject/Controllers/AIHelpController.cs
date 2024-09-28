using Microsoft.AspNetCore.Mvc;

namespace ExceptionSolutionProject.Controllers
{
    public class AIHelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendMessage(string question, IFormFile uploadedFile)
        {
            // Gelen soru ve dosya işlemleri burada yapılacak
            var answer = "AI'den gelen cevap burada olacak"; // Örnek AI cevabı
            return Json(new { answer });
        }


        private async Task<string> GetAIResponse(string question, IFormFile uploadedFile)
        {
            // AI cevabını oluşturacak bir metot. Burada AI API çağrısı yapabilirsin.
            // Yüklenen dosya (resim) varsa onu da AI'ye gönderebilirsin.

            // Şu an için basit bir mock cevap:
            return "AI'den gelen cevap: " + question;
        }

    }
}
