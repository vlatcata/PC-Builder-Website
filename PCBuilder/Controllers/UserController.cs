using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Core.Constants;
using PCBuilder.Core.Contracts;
using PCBuilder.Infrastructure.Data.Identity;

namespace PCBuilder.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserController(RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager, IUserService _userService)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            userService = _userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
