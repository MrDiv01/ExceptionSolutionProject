using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ExceptionSolutionProject.DTOs;
using ExceptionSolutionProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExceptionSolutionProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Kullanıcının token'ını al
            var token = Request.Cookies["AuthToken"]; // Cookie'den AuthToken'ı al
            UserInfoDto user = new();

            if (!string.IsNullOrEmpty(token))
            {
                // Token'ı çöz
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Gerekli verileri al
                var userName = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
                var email = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;
                var speciality = jwtToken.Claims.First(claim => claim.Type == "Speciality").Value; // Özel alan
                var phone = jwtToken.Claims.First(claim => claim.Type == "Phone").Value; // Telefon

                // Verileri ViewBag ile gönder
                user.Phone = phone;
                user.Speciality = speciality;
                user.UserEmail = email;
                user.UserName = userName;
            }

            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
