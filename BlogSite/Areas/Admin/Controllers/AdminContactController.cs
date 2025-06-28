using BlogSite.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminContactController : Controller
    {
        private readonly AppDbContext _context;

        public AdminContactController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
