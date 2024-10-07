using ExceptionSolutionProject.Data;
using ExceptionSolutionProject.DTOs;
using ExceptionSolutionProject.Models;
using ExceptionSolutionProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExceptionSolutionProject.Controllers
{
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
            List<AIChatDto> oldChats =await _context.AIChats.Select(x => new AIChatDto
            {
                 AIMessage = x.ChatMessage,
                  UserMessage = x.UserMessage,
                   CreatedTime = x.CreatingDate
            }).ToListAsync();
            return View(oldChats);
        }
        [HttpPost]
        public async Task<JsonResult> SendMessage(string message)
        {
            string response = await _openAIService.SendMessageAsync(message);


            AIChat aIChat = new()
            {
                UserMessage = message,
                ChatMessage = response

            };
            await _context.AddAsync(aIChat);
            await _context.SaveChangesAsync();

            return Json(new { success = true, openAIResponse = response, message = "Message received: " + message });
        }

    }
}
