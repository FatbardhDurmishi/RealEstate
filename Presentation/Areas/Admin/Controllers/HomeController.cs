using Microsoft.AspNetCore.Mvc;
using RealEstate.App.Constants;

namespace Presentation.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area(AreaConstants.Admin)]
        [Route("[area]/[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
