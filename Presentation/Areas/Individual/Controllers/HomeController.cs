using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Presentation.Models;
using RealEstate.App.Constants;
using RealEstate.App.Interfaces;

namespace Presentation.Areas.Individual.Controllers
{
    [Area(AreaConstants.Individual)]
    [Authorize(Roles = RoleConstants.Role_User_Comp +","+ RoleConstants.Role_User_Indi)]
    public class HomeController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserService _userService;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private IOptions<RequestLocalizationOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;
        public HomeController(IPropertyRepository propertyRepository, IUserService userService, IPropertyTypeRepository propertyTypeRepository, ITransactionTypeRepository transactionTypeRepository, ITransactionRepository transactionRepository, IHttpContextAccessor httpContextAccessor, IOptions<RequestLocalizationOptions> options)
        {
            _propertyRepository = propertyRepository;
            _userService = userService;
            _propertyTypeRepository = propertyTypeRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
            _options = options;
        }

        public IActionResult Dashboard()
        {
            if (_userService.GetUserRole() == RoleConstants.Role_User_Comp)
            {
                var userId = _userService.GetUserId();
                ViewBag.TodaySale = _transactionRepository.GetAll(x => x.Date.Day == DateTime.Today.Day && x.Owner.CompanyId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Sale, includeProperties: "Owner,TransactionTypeNavigation").Select(x => x.TotalPrice).Sum();

                ViewBag.TotalSales = _transactionRepository.GetAll(x => x.Owner.CompanyId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Sale, includeProperties: "Owner,TransactionTypeNavigation").Select(x => x.TotalPrice).Sum();

                ViewBag.RentThisMonth = _transactionRepository.GetAll(x => x.RentStartDate.Month >= DateTime.Now.Month && x.RentEndDate.Month <= DateTime.Now.Month && x.Owner.CompanyId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Rent, includeProperties: "Owner,TransactionTypeNavigation").Select(x => x.RentPrice).Sum();

                ViewBag.TotalRent = _transactionRepository.GetAll(x => x.Owner.CompanyId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Rent, includeProperties: "Owner,TransactionTypeNavigation").Select(x => x.TotalPrice).Sum();
                var latestTransactions = _transactionRepository.GetAll(x => x.Owner.CompanyId == userId || x.Buyer.CompanyId == userId, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation").OrderByDescending(x => x.Date).Take(5);

                ViewBag.Expenses = _transactionRepository.GetAll(x => x.Buyer.CompanyId == userId, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation").Select(x => x.TotalPrice).Sum();
                //ViewBag.RentThisYear
                ViewBag.BestSellThisYear = _transactionRepository.GetAll(x => x.Owner.CompanyId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Sale && x.Date.Year == DateTime.Now.Year, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation").OrderByDescending(x=>x.TotalPrice).Select(x=>x.TotalPrice).FirstOrDefault();


                ViewBag.BestRentThisYear = _transactionRepository.GetAll(x => x.Owner.CompanyId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Rent && x.Date.Year == DateTime.Now.Year, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation").OrderByDescending(x => x.RentPrice).Select(x=>x.RentPrice).FirstOrDefault();

                return View(latestTransactions);
            }
            else
            {
                var userId = _userService.GetUserId();
                ViewBag.TodaySale = _transactionRepository.GetAll(x => x.Date == DateTime.Today && x.OwnerId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Sale, includeProperties: "Owner,TransactionTypeNavigation").Select(x => x.TotalPrice).Sum();
                ViewBag.TotalSales = _transactionRepository.GetAll(x => x.OwnerId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Sale, includeProperties: "Owner,TransactionTypeNavigation").Select(x => x.TotalPrice).Sum();

                ViewBag.RentThisMonth = _transactionRepository.GetAll(x => x.RentStartDate.Month >= DateTime.Now.Month && x.RentEndDate.Month <= DateTime.Now.Month && x.OwnerId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Rent, includeProperties: "Owner,TransactionTypeNavigation").Select(x => x.RentPrice).Sum();
                ViewBag.TotalRent = _transactionRepository.GetAll(x => x.OwnerId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Rent, includeProperties: "Owner,TransactionTypeNavigation").Select(x => x.TotalPrice).Sum();
                //ViewBag.LatestTransactions = _transactionRepository.GetAll(x => x.OwnerId == userId, includeProperties: "Owner,TransactionTypeNavigation").Take(5).OrderBy(x => x.Date);
                var latestTransactions = _transactionRepository.GetAll(x => x.OwnerId == userId || x.BuyerId == userId, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation").Take(5).OrderBy(x => x.Date);
                ViewBag.Expenses = _transactionRepository.GetAll(x => x.BuyerId == userId, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation").Select(x => x.TotalPrice).Sum();
                //ViewBag.RentThisYear
                ViewBag.BestSellThisYear = _transactionRepository.GetAll(x => x.OwnerId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Sale && x.Date.Year == DateTime.Now.Year, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation").OrderByDescending(x => x.TotalPrice).Select(x => x.TotalPrice).FirstOrDefault();

                ViewBag.BestRentThisYear = _transactionRepository.GetAll(x => x.OwnerId == userId && x.TransactionTypeNavigation.Name == TransactionTypes.Rent && x.Date.Year == DateTime.Now.Year, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation").OrderByDescending(x => x.RentPrice).Select(x => x.RentPrice).FirstOrDefault();
                return View(latestTransactions);


            }

        }
        [AllowAnonymous]
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
        [AllowAnonymous]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            try
            {
                var cultureItems = _options.Value.SupportedUICultures!.Select(x => x.Name).ToArray();
                if (cultureItems.Any(x => x.Equals(culture)))
                {
                    Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = true, Secure = _httpContextAccessor.HttpContext!.Request.IsHttps });
                }
                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
