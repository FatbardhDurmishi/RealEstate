using Microsoft.AspNetCore.Mvc;
using RealEstate.App.Constants;
using RealEstate.App.Interfaces;

namespace Presentation.Areas.Company.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository, IUserService userService)
        {
            _transactionRepository = transactionRepository;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS

        public IActionResult GetTransactions()
        {
            if (_userService.GetUserRole() == RoleConstants.Role_User_Comp)
            {
                var companyId = _userService.GetUserId();
                var transactions = _transactionRepository.GetAll(x => x.Owner.CompanyId == companyId, includeProperties: "Owner,Buyer,Property");

                return Json(transactions);
            }
            return View();

            #endregion
        }
    }
}