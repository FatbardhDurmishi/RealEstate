using System.ComponentModel.Design;
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
        private readonly IUserService _userService;
        public HomeController(IPropertyRepository propertyRepository, IUserService userService)
        {
            _propertyRepository = propertyRepository;
            _userService = userService;
        }

        public IActionResult Index()
        {
            if(_userService.GetUserRole()== RoleConstants.Role_User_Comp)
            {
                var companyId= _userService.GetUserId();
                var properties = _propertyRepository.GetAll(x => x.Status == PropertyStatus.Free && x.User.CompanyId !=companyId, includeProperties: "User,PropertyTypeNavigation,TransactionTypeNavigation");
                return View(properties);
            }else
            {
                var userId = _userService.GetUserId();
                var properties = _propertyRepository.GetAll(x => x.Status == PropertyStatus.Free && x.UserId !=userId, includeProperties: "User,PropertyTypeNavigation,TransactionTypeNavigation");
                return View(properties);
            }
           
           
        }

  
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
