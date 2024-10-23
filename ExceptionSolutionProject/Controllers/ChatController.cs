using ExceptionSolutionProject.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExceptionSolutionProject.Controllers
{
    public class ChatController : Controller
    {
        private readonly HttpClient _httpClient;

        public ChatController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            List<UserResponse> users = new List<UserResponse>();

            // API URL'sini ayarla
            string apiUrl = "http://localhost:5192/api/user/";

            // API'ye GET isteği gönder
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Yanıtı JSON formatında al
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // JSON'u UserResponse modeline dönüştür
                users = JsonConvert.DeserializeObject<List<UserResponse>>(jsonResponse);
            }
            else
            {
                // Hata mesajı gösterebilirsiniz
                ViewBag.ErrorMessage = "API'den kullanıcılar alınırken bir hata oluştu.";
            }

            // Kullanıcıları View'e gönder
            return View(users);
          
        }

        [HttpPost]
        public async Task<IActionResult> StartChat()
        {
            return View();
        }
    }
}
