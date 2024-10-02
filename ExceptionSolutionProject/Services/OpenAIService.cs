using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace ExceptionSolutionProject.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "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"; // Buraya kendi API anahtarınızı koyun

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        public async Task<string> SendMessageAsync(string message)
        {
            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                new { role = "user", content = message }
            }
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse);
            return result.choices[0].message.content;
        }
    }
}
