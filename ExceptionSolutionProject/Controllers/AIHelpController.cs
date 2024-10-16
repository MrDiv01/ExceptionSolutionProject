using ExceptionSolutionProject.Data;
using ExceptionSolutionProject.DTOs;
using ExceptionSolutionProject.Helper;
using ExceptionSolutionProject.Models;
using ExceptionSolutionProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace ExceptionSolutionProject.Controllers
{
    [Authorize]

    public class AIHelpController : Controller
    {

        private readonly OpenAIService _openAIService;
        private readonly ApplicationDbContext _context;
        public AIHelpController(OpenAIService openAIService, ApplicationDbContext context)
        {
            _openAIService = openAIService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Cookie'deki token'i al
            var token = HttpContext.Request.Cookies["AuthToken"];
            if (token != null)
            {
                var userEmail = AiHelper.GetEmailFromJwt(token);
                List<AIChatDto> oldChats = await _context.AIChats.Where(_ => _.UserId == userEmail).Select(x => new AIChatDto
                {
                    AIMessage = x.ChatMessage,
                    UserMessage = x.UserMessage,
                    CreatedTime = x.CreatingDate
                }).ToListAsync();
                return View(oldChats);
            }
            else
            {
                ViewBag.NotUsed = "Helede Soru Sormamisiniz";
                return View();

            }

            // Burada e-posta adresini kullanarak daha fazla işlem yapabilirsin

        }
        [HttpPost]
        public async Task<JsonResult> SendMessage(string message)
        {
            // Cookie'deki token'i al
            var token = HttpContext.Request.Cookies["AuthToken"];

            if (token == null)
            {
                // Token yoksa 404 Not Found'u JSON formatında döndür
                return Json(new { success = false, message = "Giriş yapmalısınız. Token bulunamadı." });
            }

            // Token'dan e-posta adresini al
            var userEmail = AiHelper.GetEmailFromJwt(token);

            // OpenAI ile mesaj gönderme
            string response = await _openAIService.SendMessageAsync(message);

            // Yeni bir AIChat kaydı oluştur
            AIChat aIChat = new()
            {
                UserMessage = message,
                ChatMessage = response,
                UserId = userEmail // Token'dan alınan e-posta adresi
            };

            // Veritabanına kaydet
            await _context.AddAsync(aIChat);
            await _context.SaveChangesAsync();

            // Başarılı işlem JSON cevabı
            return Json(new { success = true, openAIResponse = response, message = "Mesaj alındı: " + message });
        }

    }
}
