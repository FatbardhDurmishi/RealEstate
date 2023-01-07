using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;
using RealEstate.App.Constants;
using RealEstate.App.Interfaces;
using RealEstate.Data.Entities;

namespace Presentation.Areas.Individual.Controllers
{
    [Area(AreaConstants.Individual)]
    public class HomeController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserService _userService;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public HomeController(IPropertyRepository propertyRepository, IUserService userService, IPropertyTypeRepository propertyTypeRepository, ITransactionTypeRepository transactionTypeRepository)
        {
            _propertyRepository = propertyRepository;
            _userService = userService;
            _propertyTypeRepository = propertyTypeRepository;
            _transactionTypeRepository = transactionTypeRepository;
        }

        public IActionResult Index(string? city, int? bedrooms, int? bathrooms, decimal? minPrice, decimal? maxPrice, int? propertyType, int? transactionType)
        {
            ViewBag.PropertyTypes = new SelectList(_propertyTypeRepository.GetAll(), "Id", "Name");
            //ViewBag.Cities = new SelectList(CityConstants._cities, "Name", "Name");
            ViewBag.TransactionType = new SelectList(_transactionTypeRepository.GetAll(), "Id", "Name");
            if (_userService.GetUserRole() == RoleConstants.Role_User_Comp)
            {
                var companyId = _userService.GetUserId();
                var properties = _propertyRepository.GetAll(x => x.Status == PropertyStatus.Free && x.User.CompanyId != companyId, includeProperties: "User,PropertyTypeNavigation,TransactionTypeNavigation");
                var filtered = properties.AsQueryable();
                filtered = city != null ? filtered.Where(x => x.City == city) : filtered;
                filtered = bedrooms.HasValue ? filtered.Where(x => x.BedRooms == bedrooms) : filtered;
                filtered = bathrooms.HasValue ? filtered.Where(x => x.BathRooms == bathrooms) : filtered;
                filtered = minPrice.HasValue ? filtered.Where(x => x.Price >= minPrice) : filtered;
                filtered = maxPrice.HasValue ? filtered.Where(x => x.Price <= maxPrice) : filtered;
                filtered = propertyType.HasValue ? filtered.Where(x => x.PropertyType == propertyType) : filtered;
                filtered = transactionType.HasValue ? filtered.Where(x => x.TransactionType == transactionType) : filtered;

                return View(filtered);

            }
            else
            {
                var userId = _userService.GetUserId();
                var properties = _propertyRepository.GetAll(x => x.Status == PropertyStatus.Free && x.UserId != userId, includeProperties: "User,PropertyTypeNavigation,TransactionTypeNavigation");
                var filtered = properties.AsQueryable();
                filtered = city != null ? filtered.Where(x => x.City == city) : filtered;
                filtered = bedrooms.HasValue ? filtered.Where(x => x.BedRooms == bedrooms) : filtered;
                filtered = bathrooms.HasValue ? filtered.Where(x => x.BathRooms == bathrooms) : filtered;
                filtered = minPrice.HasValue ? filtered.Where(x => x.Price >= minPrice) : filtered;
                filtered = maxPrice.HasValue ? filtered.Where(x => x.Price <= maxPrice) : filtered;
                filtered = propertyType.HasValue ? filtered.Where(x => x.PropertyType == propertyType) : filtered;
                filtered = transactionType.HasValue ? filtered.Where(x => x.TransactionType == transactionType) : filtered;

                return View(filtered);
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
