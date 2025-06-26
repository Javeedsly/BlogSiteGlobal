using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
