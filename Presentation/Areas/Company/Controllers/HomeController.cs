using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Company.Controllers
{
    public class HomeController : Controller
    {
        [Area("Company")]
        [Route("[area]/[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
