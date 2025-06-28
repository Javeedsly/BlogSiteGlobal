using BlogSite.Core.Models;
using BlogSite.Business.Services.Abstract;
using BlogSite.Data.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;

        public ContactController(AppDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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

            try
            {
                string body = $"Name: {message.Name}\nEmail: {message.Email}\n\nMessage:\n{message.Message}";

                await _emailSender.SendEmailAsync("cavidsly@gmail.com", message.Subject, body);

                _context.ContactMessages.Add(message);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Mesajınız uğurla göndərildi!";
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Email göndərərkən xəta baş verdi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
