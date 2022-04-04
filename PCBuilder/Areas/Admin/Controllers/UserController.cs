using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCBuilder.Core.Constants;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models;
using PCBuilder.Infrastructure.Data.Identity;

namespace PCBuilder.Areas.Admin.Controllers
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

        public async Task<IActionResult> ManageUsers()
        {
            var users = await userService.GetUsers();

            return View(users);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await userService.GetUserById(id);

            var model = new UserRolesViewModel()
            {
                UserId = user.Id,
                Name = $"{user.FirstName} {user.LastName}"
            };

            ViewBag.RoleItems = roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Id,
                    Selected = userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Roles(UserRolesViewModel model)
        {
            var user = await userService.GetUserById(model.UserId);
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleNames?.Length > 0)
            {
                await userManager.AddToRolesAsync(user, model.RoleNames);
            }

            return RedirectToAction("ManageUsers");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await userService.GetUserToEdit(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await userService.UpdateUser(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "User Information was updated";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "User Information was not updated";
            }

            return View(model);
        }

        public async Task<IActionResult> CreateRole()
        {
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Administrator"
            //});

            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Guest"
            //});

            return Ok();
        }
    }
}
