using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Areas.Admin.Models.PropertyTypeVM;
using Presentation.Areas.Admin.Models.TransactionTypeVM;
using RealEstate.App.Constants;
using RealEstate.App.Implementations;
using RealEstate.App.Interfaces;
using RealEstate.Data.Entities;

namespace Presentation.Areas.Admin.Controllers
{
    [Area(AreaConstants.Admin)]
    [Authorize(Roles = RoleConstants.Role_Admin)]
    public class TransactionTypesController : Controller
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public TransactionTypesController(ITransactionTypeRepository transactionTypeRepository)
        {
            _transactionTypeRepository = transactionTypeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            TransactionTypeVM typeVM = new()
            {
            };
            if (id == null || id == 0)
            {
                return View(typeVM);
            }
            else
            {
                TransactionsType? type = _transactionTypeRepository.GetFirstOrDefault(x => x.Id == id);
                if (type != null)
                {
                    var typemodel = new TransactionTypeVM()
                    {

                        Name = type.Name!,


                    };
                    return View(typemodel);
                }
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TransactionTypeVM model)
        {
            if (ModelState.IsValid)
            {

                if (model.Id == 0)
                {
                    var type = new TransactionsType()
                    {
                        Name = model.Name!,
                    };
                    _transactionTypeRepository.Add(type);
                    TempData["success"] = "Property type  added successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var type = _transactionTypeRepository.GetFirstOrDefault(x => x.Id == model.Id);
                    if (type != null)
                    {
                        type.Name = model.Name!;
                        _transactionTypeRepository.Update(type);
                        TempData["success"] = "Property type  updated successfully";
                        return RedirectToAction(nameof(Index));
                    }

                }
            }

            return View(model);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetTransactionTypesJson()
        {
            var transactionTypes = _transactionTypeRepository.GetAll();
            return new JsonResult(transactionTypes);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _transactionTypeRepository.GetFirstOrDefault(x => x.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _transactionTypeRepository.Remove(obj);
            return Json(new { success = true, message = "Deleted Successfully" });

        }
        #endregion
    }
}
