using ExceptionSolutionProject.Data;
using ExceptionSolutionProject.Helper;
using ExceptionSolutionProject.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Transactions;
using MailKit.Net.Smtp;
using MimeKit;
using ExceptionSolutionProject.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ExceptionSolutionProject.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        //TO DO : HangFire Elave olunacaq Istifadeci Mailini daxil etdikden sonra
        //Zamani geldikde o maile Bildiris gedecek Iclas barede melumat verilecek.
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> List()
        {
            var token = HttpContext.Request.Cookies["AuthToken"];

            var userEmail = AiHelper.GetEmailFromJwt(token);

            List<NotListDto> notes = await _context.ScheduledMessage
                                    .Where(x => x.UserEmail == userEmail)
                                        .Select(x => new NotListDto
                                        {
                                            Date = x.ScheduledTime,
                                            Note = x.Message
                                        }).ToListAsync();

            return View(notes);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ScheduleMessage(string message, DateTime scheduledTime)
        {
            var token = HttpContext.Request.Cookies["AuthToken"];

            var userEmail = AiHelper.GetEmailFromJwt(token);

            // Yeni bir ScheduledMessage oluştur
            var scheduledMessage = new ScheduledMessage
            {
                UserEmail = userEmail,
                Message = message,
                ScheduledTime = scheduledTime,
                IsSent = false // Başlangıçta gönderilmedi
            };

            // Mesajı veritabanına ekleyin
            _context.ScheduledMessage.Add(scheduledMessage);
            _context.SaveChanges();

            // Mesajı planla
            var sendTime = scheduledTime.AddHours(-2);
            BackgroundJob.Schedule(() => SendEmail(userEmail, message, scheduledTime), sendTime);

            return RedirectToAction("List", "Notes");
        }
        public void SendEmail(string userEmail, string message, DateTime time)
        {
            // E-posta mesajını oluşturun
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ExpSolution.com", "nurlan.memmedov8818@gmail.com")); // Gönderen bilgisi
            emailMessage.To.Add(new MailboxAddress("Alıcı Adı", userEmail)); // Alıcı bilgisi
            emailMessage.Subject = "Planlanan Mesaj"; // E-posta konusu

            // E-posta içeriği
            emailMessage.Body = new TextPart("html") // HTML formatında içerik
            {
                Text = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #ffffff; /* Beyaz arka plan */
                    color: #333333; /* Koyu gri metin rengi */
                    padding: 20px;
                }}
                .header {{
                    background-color: #007BFF; /* Mavi arka plan */
                    color: white; /* Beyaz metin rengi */
                    padding: 10px;
                    text-align: center;
                }}
                .content {{
                    margin-top: 20px;
                }}
            </style>
        </head>
        <body>
            <div class='header'>
                <h1><--{time}-->  tarihde <--{message}--> boyle bir notunuz vardi :)</h1>
            </div>
            <div class='content'>
                <p></p>
            </div>
        </body>
        </html>
        "
            };

            // SMTP ayarlarını yapın
            using (var client = new SmtpClient())
            {
                // SMTP sunucusuna bağlanın
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Gmail için kimlik doğrulama
                client.Authenticate("nurlan.memmedov8818@gmail.com", "ygvo vots kdhc eyrn"); // Kullanıcı adı ve parola

                // E-postayı gönder
                client.Send(emailMessage);
                client.Disconnect(true); // Bağlantıyı kapat
            }

            // Mesaj gönderim işlemi tamamlandıktan sonra veritabanını güncelleyin
            using (var scope = new TransactionScope())
            {
                // Mesajı veritabanından bul
                var scheduledMessage = _context.ScheduledMessage
                    .FirstOrDefault(m => m.UserEmail == userEmail && m.Message == message && !m.IsSent);

                if (scheduledMessage != null)
                {
                    // IsSent değerini true yap
                    scheduledMessage.IsSent = true;
                    _context.SaveChanges();
                }

                // İşlem tamamlandığında transaction'ı tamamlayın
                scope.Complete();
            }
        }

    }
}
