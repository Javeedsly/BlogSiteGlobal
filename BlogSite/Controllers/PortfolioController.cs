using BlogSite.Data.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;

        public PortfolioController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var portfolios = _context.Portfolio.ToList();
            return View(portfolios);
        }

    }
}
