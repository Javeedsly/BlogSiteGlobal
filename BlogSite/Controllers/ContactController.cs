using BlogSite.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using BlogSite.Data;
using BlogSite.Data.DAL;

namespace BlogSite.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ContactController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", message);
            }

            // Verilənlər bazasına əlavə et
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            try
            {
                // Email göndər
                var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Smtp:Port"]),
                    Credentials = new System.Net.NetworkCredential(
                        _configuration["Smtp:Email"],
                        _configuration["Smtp:Password"]
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Smtp:Email"]),
                    Subject = message.Subject,
                    Body = $"Name: {message.Name}\nEmail: {message.Email}\n\nMessage:\n{message.Message}",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add("cavidsly@gmail.com");

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (System.Exception ex)
            {
                // Əgər email göndərərkən xəta olsa, log və ya mesaj verə bilərsən
                TempData["Error"] = "Email göndərərkən xəta baş verdi: " + ex.Message;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Mesajınız uğurla göndərildi!";
            return RedirectToAction("Index");
        }
    }
}
