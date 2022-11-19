using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Areas.Admin.Models.UserVM;
using Presentation.Areas.Individual.Models.AccountViewModels;
using RealEstate.App.Constants;
using RealEstate.App.Interfaces;
using RealEstate.Data.Entities;
using RealEstate.Data.Identity;
using System.Security.Claims;

namespace Presentation.Areas.Admin.Controllers
{
    [Area(AreaConstants.Admin)]
    //[Route("[area]/[controller]/[action]")]
    [Authorize(Roles = RoleConstants.Role_Admin + "," + RoleConstants.Role_User_Comp)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolesRepository _rolesRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UserController(IUserRepository userRepository, IUserService userService, RoleManager<IdentityRole> roleManager, IRolesRepository rolesRepository, UserManager<ApplicationUser> userManager, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _userService = userService;
            _roleManager = roleManager;
            _rolesRepository = rolesRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(string? id)
        {
            UserVM userVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            if (id == null || id == "0")
            {
                return View(userVM);
            }
            else
            {
                AspNetUser? user = _userRepository.GetByStringId(id);
                if (user != null)
                {
                    var model = new UserVM()
                    {
                        Id = id,
                        Name = user.Name!,
                        Password = "",
                        ConfirmPassword = "",
                        PhoneNumber = user.PhoneNumber,
                        Role = _rolesRepository.GetByUserId(user.Id)!.Name,
                        Email = user.Email!,
                        State = user.State,
                        StreetAddres = user.StreetAddres,
                        City = user.City,
                        PostalCode = user.PostalCode,
                        RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                        {
                            Text = i,
                            Value = i
                        })
                    };
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(UserVM model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };

                    user.StreetAddres = model.StreetAddres;
                    user.City = model.City;
                    user.State = model.State;
                    user.PostalCode = model.PostalCode;
                    user.Name = model.Name;
                    user.PhoneNumber = model.PhoneNumber;
                    if (_userService.GetUserRole() == RoleConstants.Role_User_Comp)
                    {
                        user.Role = RoleConstants.Role_User_Indi;
                    }
                    else
                    {
                        user.Role = model.Role;
                    }

                    if (_userService.GetUserRole() == RoleConstants.Role_User_Comp)
                    {
                        user.CompanyId = _userService.GetUserId();
                    }
                    var result = await _userManager.CreateAsync(user, model.Password);


                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");
                        if (user.Role != null)
                        {
                            await _userManager.AddToRoleAsync(user, user.Role);
                        }

                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
                        //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                        //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        //{
                        //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        //}
                        //else
                        //{
                        if (_userService.GetUserRole() == RoleConstants.Role_Admin || _userService.GetUserRole() == RoleConstants.Role_User_Comp)
                        {
                            TempData["success"] = "New User Created Successfuly";
                            //_logger.LogInformation("User created a new account with password.");
                            return RedirectToAction(nameof(Index));
                        }

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Error while adding User";
                        return View(model);
                    }
                }
                else
                {
                    var user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        user.Name = model.Name;
                        user.Email = model.Email;
                        user.PhoneNumber = model.PhoneNumber;
                        user.City = model.City;
                        user.State = model.State;
                        user.StreetAddres = model.StreetAddres;
                        user.Role = model.Role;
                        user.PostalCode = model.PostalCode;

                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            var currentRole = _rolesRepository.GetByUserId(user.Id);
                            if (currentRole != null && currentRole.Name != model.Role)
                            {
                                var roleResult = await _userManager.RemoveFromRoleAsync(user, currentRole.Name);
                                if (roleResult.Succeeded)
                                {
                                    await _userManager.AddToRoleAsync(user, model.Role);
                                    return RedirectToAction(nameof(Index));
                                }
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetUsersJson()
        {

            if (_userService.GetUserRole() == RoleConstants.Role_Admin)
            {
                var users = _userRepository.GetAll();
                var result = users.Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    email = x.Email,
                    phoneNumber = x.PhoneNumber,
                    emailConfirmed = x.EmailConfirmed,
                    role = x.Role,

                });
                return new JsonResult(result);
            }
            else
            {
                var userId = _userService.GetUserId();
                var users = _userRepository.GetAll(x => x.CompanyId == userId);
                var result = users.Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    email = x.Email,
                    phoneNumber = x.PhoneNumber,
                    emailConfirmed = x.EmailConfirmed,
                    role = x.Role,
                });
                return new JsonResult(result);

            }
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var obj = _userRepository.GetByStringId(id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _userRepository.Remove(obj);
            _userRepository.SaveChanges();
            return Json(new { success = true, message = "Deleted Successfully" });

        }


        #endregion
    }
}
