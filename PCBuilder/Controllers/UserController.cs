using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Core.Constants;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models;
using PCBuilder.Infrastructure.Data.Identity;

namespace PCBuilder.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        private readonly IComputerService computerService;

        public UserController(RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager, IUserService _userService, IComputerService _computerService)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            userService = _userService;
            computerService = _computerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            var computers = await computerService.GetUserComputers(user.Id);

            return View(computers);
        }

        public async Task<IActionResult> EditProfile()
        {
            var user = await userManager.GetUserAsync(User);
            var appUser = await userService.GetUserToEdit(user.Id);

            return View(appUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await userService.UpdateUser(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Your profile was updated";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "An error occured while trying to update your profile";
            }

            return View(model);
        }
    }
}
