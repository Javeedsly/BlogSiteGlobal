using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
