using BlogSite.Areas.Admin.ViewModels;
using BlogSite.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.ContactMessages.OrderByDescending(m => m.CreatedAt).Take(5).ToList();
            var portfolios = _context.Portfolio.OrderByDescending(p => p.Id).Take(5).ToList();

            var vm = new DashboardVM
            {
                ContactMessages = messages,
                Portfolios = portfolios
            };

            return View(vm);
        }
    }
}
