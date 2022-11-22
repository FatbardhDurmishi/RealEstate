using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Areas.Admin.Models.PropertyTypeVM;
using Presentation.Areas.Company.Models.PropertyVM;
using RealEstate.App.Constants;
using RealEstate.App.Implementations;
using RealEstate.App.Interfaces;
using RealEstate.Data.Entities;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Presentation.Areas.Company.Controllers
{
    [Area(AreaConstants.Company)]
    [Authorize(Roles = RoleConstants.Role_User_Indi + "," + RoleConstants.Role_User_Comp)]
    public class PropertiesController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserService _userService;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PropertiesController(IPropertyRepository propertyRepository, IUserService userService, ITransactionTypeRepository transactionTypeRepository, IUserRepository userRepository, IPropertyTypeRepository propertyTypeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _propertyRepository = propertyRepository;
            _userService = userService;
            _transactionTypeRepository = transactionTypeRepository;
            _userRepository = userRepository;
            _propertyTypeRepository = propertyTypeRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                PropertyVM propertyVM = new()
                {
                    Property = new(),
                    TransactionList = _transactionTypeRepository.GetAll().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    }),
                    PropertyTypeList = _propertyTypeRepository.GetAll().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    UsersList = _userRepository.GetAll(x => x.CompanyId == _userService.GetUserId()).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };

                return View(propertyVM);
            }
            else
            {

                var model = new PropertyVM()
                {
                    Property = _propertyRepository.GetFirstOrDefault(x => x.Id == id, includeProperties: "User,TransactionTypeNavigation,PropertyTypeNavigation,PropertyImages"),
                    TransactionList = _transactionTypeRepository.GetAll().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    PropertyTypeList = _propertyTypeRepository.GetAll().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),

                    UsersList = _userRepository.GetAll(x => x.CompanyId == _userService.GetUserId()).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };
                return View(model);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PropertyVM model)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (model.CoverImage != null)
                {

                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Images/Property/PropertyCoverPhoto");
                    var extension = Path.GetExtension(model.CoverImage.FileName);
                    if (model.Property.CoverImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, model.Property.CoverImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        model.CoverImage.CopyTo(fileStreams);
                    }
                    model.Property.CoverImageUrl = @"\Images\Property\PropertyCoverPhoto\" + filename + extension;
                }
                if (model.PropertyImages != null)
                {
                    foreach (var item in model.PropertyImages)
                    {
                        string filename = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"Images/Property/PropertyImages");
                        var extension = Path.GetExtension(item.FileName);

                        using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                        {
                            item.CopyTo(fileStreams);
                        }
                        var Image = new PropertyImage()
                        {
                            ImageUrl = @"\Images\Property\PropertyImages\" + filename + extension
                        };
                        model.Property.PropertyImages.Add(Image);
                    }

                }
                model.Property.Status = PropertyStatus.Free;
                if (_userService.GetUserRole() == RoleConstants.Role_User_Indi)
                {
                    model.Property.UserId = _userService.GetUserId();
                }
                if (model.Property.Id == 0)
                {
                    _propertyRepository.Add(model.Property);
                    TempData["success"] = "Property added successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _propertyRepository.Update(model.Property);
                    TempData["success"] = "Property updated successfully";
                    return RedirectToAction(nameof(Index));

                }
            }

            return View(model);
        }


        #region API CALLS
        public IActionResult GetPropertiesJson()
        {

            if (_userService.GetUserRole() == RoleConstants.Role_User_Comp)
            {
                var companyId = _userService.GetUserId();
                var properties = _propertyRepository.GetAll(x => x.User.CompanyId == companyId, includeProperties: "User,PropertyTypeNavigation,TransactionTypeNavigation");
                //var result = propeties.Select(x => new
                //{
                //    id = x.Id,
                //    name = x.Name,
                //    user = x.User.Name,
                //    propertyType = x.PropertyTypeNavigation.Name,
                //    transactionType = x.TransactionTypeNavigation.Name,
                //    price = x.Price,
                //    status = x.Status
                //});
                return Json(properties);
            }
            else
            {
                var userId = _userService.GetUserId();
                var properties = _propertyRepository.GetAll(x => x.UserId == userId, includeProperties: "PropertyTypeNavigation,TransactionTypeNavigation");
                //var result = properties.Select(x => new
                //{
                //    id = x.Id,
                //    name = x.Name,
                //    user = x.User.Name,
                //    propertyType = x.PropertyTypeNavigation.Name,
                //    transactionType = x.TransactionTypeNavigation.Name,
                //    price = x.Price,
                //    status = x.Status

                //});
                return Json(properties);

            }


        }

        public IActionResult Details(int propertyId)
        {
            Property obj = _propertyRepository.GetFirstOrDefault(x => x.Id == propertyId, includeProperties: "User,PropertyTypeNavigation,TransactionTypeNavigation,PropertyImages");
            return View(obj);
        }

        #endregion

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }
    }

}

