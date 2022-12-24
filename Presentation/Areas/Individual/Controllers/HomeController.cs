using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using RealEstate.App.Constants;
using RealEstate.App.Interfaces;

namespace Presentation.Areas.Individual.Controllers
{
    [Area(AreaConstants.Individual)]
    public class HomeController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;

        public HomeController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public IActionResult Index()
        {
            var properties = _propertyRepository.GetAll(x => x.Status == PropertyStatus.Free,includeProperties: "User,PropertyTypeNavigation,TransactionTypeNavigation");
            return View(properties);
        }

  
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
