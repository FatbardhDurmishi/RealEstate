using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Areas.Company.Models.TransactionVM;
using RealEstate.App.Constants;
using RealEstate.App.Interfaces;
using RealEstate.Data.Entities;

namespace Presentation.Areas.Company.Controllers
{
    [Area(AreaConstants.Company)]
    [Authorize(Roles = RoleConstants.Role_User_Indi + "," + RoleConstants.Role_User_Comp)]
    public class TransactionController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPropertyRepository _propertyRepository;

        public TransactionController(ITransactionRepository transactionRepository, IUserService userService, IUserRepository userRepository, IPropertyRepository propertyRepository)
        {
            _transactionRepository = transactionRepository;
            _userService = userService;
            _propertyRepository = propertyRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddTransaction(int? propertyId)
        {
            TransactionVM transaction = new()
            {
                Property = _propertyRepository.GetFirstOrDefault(x => x.Id == propertyId, includeProperties: "PropertyTypeNavigation,TransactionTypeNavigation"),
                Transaction = new Transaction()
            };
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddTransaction(TransactionVM model)
        {
            if(ModelState.IsValid)
            {
                _transactionRepository.Add(model.Transaction);
                TempData["success"] = "Request is made successfully";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Something went wrong! Try Again";
                return View(model);

            }

        }


        #region API CALLS

        public IActionResult GetTransactions()
        {
            if (_userService.GetUserRole() == RoleConstants.Role_User_Comp)
            {
                var companyId = _userService.GetUserId();
                var transactions = _transactionRepository.GetAll(x => x.Owner.CompanyId == companyId, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation");

                return Json(transactions);
            }
            else
            {
                var userId = _userService.GetUserId();
                var transactions = _transactionRepository.GetAll(x => x.OwnerId == userId || x.BuyerId == userId, includeProperties: "Owner,Buyer,Property,TransactionTypeNavigation");

                return Json(transactions);
            }
            #endregion
        }
    }
}