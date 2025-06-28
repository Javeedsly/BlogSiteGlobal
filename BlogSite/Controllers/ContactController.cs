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
        public async Task<IActionResult> Send(ContactMessage contactMessage) // Renamed parameter to avoid conflict  
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", contactMessage);
            }

            try
            {
                string body = $"Name: {contactMessage.Name}\nEmail: {contactMessage.Email}\n\nMessage:\n{contactMessage.Message}";

                await _emailSender.SendEmailAsync("cavidsly@gmail.com", contactMessage.Subject, body);

                _context.ContactMessages.Add(contactMessage);
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
