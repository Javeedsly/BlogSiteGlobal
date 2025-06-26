using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
