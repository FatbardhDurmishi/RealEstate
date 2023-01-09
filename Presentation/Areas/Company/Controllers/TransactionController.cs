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

        public TransactionController(ITransactionRepository transactionRepository, IUserService userService, IPropertyRepository propertyRepository)
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
        [Authorize(Roles = RoleConstants.Role_User_Indi)]
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
        [Authorize(Roles = RoleConstants.Role_User_Indi)]
        public IActionResult AddTransaction(TransactionVM model)
        {
            var userId = _userService.GetUserId();
            var transaction = _transactionRepository.GetFirstOrDefault(x => x.BuyerId == userId && x.PropertyId == model.Property.Id && x.Status != TransactionStatus.Denied, includeProperties: "Property,TransactionTypeNavigation");
           
            if (transaction == null || transaction.Status == TransactionStatus.Denied)
            {
                if (ModelState.IsValid)
                {
                    model.Transaction.Status = TransactionStatus.Pending;
                    model.Transaction.Date = DateTime.Now;
                    model.Transaction.OwnerId = model.Property.UserId;
                    model.Transaction.BuyerId = _userService.GetUserId();
                    model.Transaction.PropertyId = model.Property.Id;
                    model.Transaction.TransactionType = model.Property.TransactionType;

                    if (model.Property.TransactionTypeNavigation.Name == TransactionTypes.Rent)
                    {
                        model.Transaction.TotalPrice = model.Property.Price * CalculateTotalPrice(model.Transaction.RentStartDate, model.Transaction.RentEndDate);
                        model.Transaction.RentPrice = model.Property.Price;
                    }
                    else
                    {
                        model.Transaction.TotalPrice = model.Property.Price;
                    }


                    _transactionRepository.Add(model.Transaction);
                    TempData["success"] = "Request is made successfully";
                    return RedirectToAction("Index", "Home", new { area = "Individual" });
                }
                else
                {
                    TempData["error"] = "Something went wrong! Try Again";
                    return RedirectToAction("Index", "Home", new { area = "Individual" });
                }
            }
            else
            {
                if (transaction.TransactionTypeNavigation.Name == TransactionTypes.Rent)
                {
                    TempData["error"] = "You already requested to rent this property";
                    return RedirectToAction("Index", "Home", new { area = "Individual" });
                }
                else
                {
                    TempData["error"] = "You already requested to buy this property";
                    return RedirectToAction("Index", "Home", new { area = "Individual" });
                }

            }


        }

        public static int CalculateTotalPrice(DateTime startDate, DateTime endDate)
        {
            int months = 0;
            while (startDate < endDate)
            {
                startDate = startDate.AddMonths(1);
                months++;
            }
            return months;
        }

        public IActionResult ApproveRequest(int id)
        {
            var transaction = _transactionRepository.GetFirstOrDefault(x => x.Id == id, includeProperties: "TransactionTypeNavigation,Buyer,Owner,Property");
            var property = _propertyRepository.GetFirstOrDefault(x => x.Id == transaction.PropertyId);

            if (transaction.TransactionTypeNavigation.Name == TransactionTypes.Rent)
            {
                _transactionRepository.UpdateStatus(transaction, TransactionStatus.Rented);
                _transactionRepository.SaveChanges();
                _propertyRepository.UpdateStatus(property, PropertyStatus.Rented);
                _propertyRepository.SaveChanges();
                return View(nameof(Index));

            }
            else
            {
                _transactionRepository.UpdateStatus(transaction, TransactionStatus.Sold);
                transaction.Property.UserId = transaction.BuyerId;
                _transactionRepository.SaveChanges();
                return View(nameof(Index));
            }

        }


        public IActionResult RejectRequest(int id)
        {
            var transaction = _transactionRepository.GetFirstOrDefault(x => x.Id == id);
            _transactionRepository.UpdateStatus(transaction, TransactionStatus.Denied);
            _transactionRepository.SaveChanges();
            return View(nameof(Index));
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
                foreach (var transaction in transactions)
                {
                    if (transaction.OwnerId == userId && transaction.Status != TransactionStatus.Sold && transaction.Status != TransactionStatus.Rented && transaction.Status != TransactionStatus.Denied && transaction.Status != TransactionStatus.Expired)
                    {
                        transaction.ShowButtons = true;
                    }
                }

                return Json(transactions);
            }


        }


        public IActionResult Details(int id)
        {
            var transaction = _transactionRepository.GetFirstOrDefault(x => x.Id == id, includeProperties: "TransactionTypeNavigation,Buyer,Owner,Property");
            TransactionVM transactionvm = new()
            {
                Property = _propertyRepository.GetFirstOrDefault(x => x.Id == transaction.PropertyId, includeProperties: "PropertyTypeNavigation,TransactionTypeNavigation"),
                Transaction = _transactionRepository.GetFirstOrDefault(x => x.Id == id, includeProperties: "TransactionTypeNavigation,Buyer,Owner,Property")
            };

            return View(transactionvm);

        }

        public IActionResult Delete(int? id)
        {
            var transaction = _transactionRepository.GetFirstOrDefault(x => x.Id == id);
            if (transaction == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _transactionRepository.Remove(transaction);
            return Json(new { success = true, message = "Deleted Successfully" });
        }


        #endregion
    }
}