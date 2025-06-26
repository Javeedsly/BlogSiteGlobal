using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class ResumeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
