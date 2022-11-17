using Microsoft.AspNetCore.Mvc;
using RealEstate.App.Constants;
using RealEstate.App.Interfaces;
using RealEstate.Data.Entities;
using System.Security.Claims;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetUsersJson()
        {

            if (_userService.GetUserRole()== RoleConstants.Role_Admin)
            {
                var users = _userRepository.GetAll();
                var result = users.Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    email = x.Email,
                    phoneNumber = x.PhoneNumber,
                    emailConfirmed = x.EmailConfirmed,
                });
                return new JsonResult(result);
            }
            else
            {
                var userId  = _userService.GetUserId();
                var users = _userRepository.GetAll(x=>x.CompanyId==userId);
                var result = users.Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    email = x.Email,
                    phoneNumber = x.PhoneNumber,
                    emailConfirmed = x.EmailConfirmed,
                });
                return new JsonResult(result);

            }



        }
    }
}
