using System;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using ExpHub.DTOs;

namespace ExpHub.Services
{
    public  class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Login kontrolü
        public async Task<bool> LoginCheck(LoginDto loginDto)
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5192/api/auth/");

            // API'ye login isteği gönder
            var response = await _httpClient.PostAsJsonAsync("login", loginDto);

            // Eğer durum kodu 200 ise true döndür
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

                // Token'ı işleyebilirsin (controller'da)
                var token = tokenResponse?.token?.result;

                // Başarılı
                return true;
            }
            else
            {
                // Başarısız
                return false;
            }
        }
    }


    public class TokenResponse
    {
        public TokenResult token { get; set; }
    }

    public class TokenResult
    {
        public string result { get; set; }
    }
}