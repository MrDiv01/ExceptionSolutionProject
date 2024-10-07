using ExceptionSolutionProject.DTOs;
using ExceptionSolutionProject.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExceptionSolutionProject.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginRegisterController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            _httpClient.BaseAddress = new Uri("http://localhost:5192/api/auth/");

            var response = await _httpClient.PostAsJsonAsync("login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

                var token = tokenResponse.token.result;

                HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Mail or Password InCorrect"); 
                return View(loginDto); 
            }
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5192/api/auth/register", model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            return View(model); 
        }

        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpContext.Response.Cookies.Delete("AuthToken");
                return RedirectToAction("Login"); 

            }
            else
            {
                ViewBag.ErrorMessage = "Çıkış işlemi sırasında bir hata oluştu.";
            }

            return View();
        }


    }
}
