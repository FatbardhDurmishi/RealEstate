using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Areas.Admin.Models.PropertyTypeVM;
using Presentation.Areas.Admin.Models.UserVM;
using RealEstate.App.Constants;
using RealEstate.App.Implementations;
using RealEstate.App.Interfaces;
using RealEstate.Data.Entities;
//using RealEstate.Data.Migrations;
using System.Xml;

namespace Presentation.Areas.Admin.Controllers
{
    [Area(AreaConstants.Admin)]
    [Authorize(Roles = RoleConstants.Role_Admin)]
    public class PropertyTypesController : Controller
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public PropertyTypesController(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            PropertyTypeVM typeVM = new()
            {
            };
            if (id == null || id == 0)
            {
                return View(typeVM);
            }
            else
            {
                PropertyType? type = _propertyTypeRepository.GetFirstOrDefault(x => x.Id == id);
                if (type != null)
                {
                    var typemodel = new PropertyTypeVM()
                    {
                        Id = type.Id,
                        Name = type.Name!,


                    };
                    return View(typemodel);
                }
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PropertyTypeVM model)
        {
            if (ModelState.IsValid)
            {

                if (model.Id == 0)
                {
                    var type = new PropertyType()
                    {
                        Name = model.Name!,
                    };
                    _propertyTypeRepository.Add(type);
                    TempData["success"] = "Property type  added successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var type = _propertyTypeRepository.GetFirstOrDefault(x => x.Id == model.Id);
                    if (type != null)
                    {
                        type.Name = model.Name!;
                        _propertyTypeRepository.Update(type);
                        TempData["success"] = "Property type  updated successfully";
                        return RedirectToAction(nameof(Index));
                    }

                }
            }

            return View(model);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetPropertyTypesJson()
        {
            var propetyTypes = _propertyTypeRepository.GetAll();
            return new JsonResult(propetyTypes);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _propertyTypeRepository.GetFirstOrDefault(x => x.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _propertyTypeRepository.Remove(obj);
            return Json(new { success = true, message = "Deleted Successfully" });

        }
        #endregion
    }
}

