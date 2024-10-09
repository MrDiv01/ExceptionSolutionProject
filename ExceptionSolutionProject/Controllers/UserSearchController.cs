using ExceptionSolutionProject.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ExceptionSolutionProject.Controllers
{
    public class UserSearchController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserSearchController(HttpClient httpClient)
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
        public async Task<IActionResult> SendMessage(string userId)
        {
            // API URL'sini düzəldin
            string apiUrl = $"http://localhost:5192/api/user/{userId}";

            try
            {
                // API'ye GET isteği gönder
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Yanıtı JSON formatında al
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // JSON'u UserResponse modeline dönüştür
                    var userResponse = JsonConvert.DeserializeObject<UserByIdResponse>(jsonResponse);

                    // Burada istədiyin loqikanı əlavə et (məsələn, mesaj göndər)
                    // Mesaj göndərmə kodunu bura əlavə edə bilərsən

                    return View(userResponse); // JSON nəticəsini qaytar
                }
                else
                {
                    // Hata durumunda bir hata mesajı qaytar
                    return NotFound(new { message = "Kullanıcı bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                // Xətanı logla və müvafiq mesaj ver
                return StatusCode(500, new { message = "Sunucu xətası baş verdi.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string userId, string message)
        {
            // Mesaj gönderme kodunu buraya ekleyebilirsiniz
            // Örnek: bir mesaj gönderme API'sine istekte bulunma

            var apiUrl = $"http://localhost:5192/api/sendMessage"; // Mesaj gönderim API'sinin URL'si
            var content = new StringContent(JsonConvert.SerializeObject(new { UserId = userId, Message = message }), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.ResponseMessage = "Mesaj başarıyla gönderildi.";
                }
                else
                {
                    ViewBag.ResponseMessage = "Mesaj gönderilirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda uygun bir mesaj ver
                ViewBag.ResponseMessage = "Sunucu hatası: " + ex.Message;
            }

            return View(); // Kullanıcı bilgilerini ve yanıt mesajını döndür
        }

    }
}
