using ExceptionSolutionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionSolutionProject.Controllers
{
    public class AIHelpController : Controller
    {

        private readonly OpenAIService _openAIService;

        public AIHelpController(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> SendMessage(string message)
        {
            // OpenAI API'ye istek gönderip yanıtı alıyoruz
            string response = await _openAIService.SendMessageAsync(message)/*"Bu mesaj AI den gelib"*/;
            
            // İstemciye OpenAI yanıtını dönüyoruz
            return Json(new { success = true, openAIResponse = response, message = "Message received: " + message });
        }

    }
}
