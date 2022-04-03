using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Core.Constants;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models.Cart;
using PCBuilder.Infrastructure.Common;
using PCBuilder.Infrastructure.Data.Identity;

namespace PCBuilder.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService cartService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartService _cartService, IUserService _userService, UserManager<ApplicationUser> _userManager)
        {
            userService = _userService;
            cartService = _cartService;
            userManager = _userManager;
        }

        public async Task<IActionResult> Cart()
        {
            var user = await userManager.GetUserAsync(User);
            var cart = await cartService.GetCartComponents(user.Id);

            if (cart.Components.Count < 7)
            {
                ViewData[MessageConstant.ErrorMessage] = "You need to have every component to build a PC";
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "You have everything needed to build a computer (press the \"Build PC\" button)";
            }

            return View(cart);
        }

        [Authorize(Roles = UserConstants.Roles.Administrator)]
        public async Task<IActionResult> AddComponent()
        {
            var model = await cartService.GenerateDefaultModel();

            return View(model);
        }

        [Authorize(Roles = UserConstants.Roles.Administrator)]
        [HttpPost]
        public async Task<IActionResult> AddComponent(AddComponentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (await cartService.CreateComponent(viewModel))
            {
                ViewData[MessageConstant.SuccessMessage] = "Component was created successfully!";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
            }

            return View(viewModel);
        }

        public async Task<IActionResult> DetailsComponent(string id)
        {
            var component = await cartService.GetComponent(id);

            return View(component);
        }

        public async Task<IActionResult> EditComponent(string id)
        {
            var component = await cartService.GetComponent(id);

            return View(component);
        }

        [Authorize(Roles = UserConstants.Roles.Administrator)]
        [HttpPost]
        public async Task<IActionResult> EditComponent(AddComponentViewModel viewModel)
        {
            if (await cartService.EditComponent(viewModel))
            {
                ViewData[MessageConstant.SuccessMessage] = "Component Information was updated";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Component Information was not updated";
            }

            return View(viewModel);
        }

        [Authorize(Roles = UserConstants.Roles.Administrator)]
        public async Task<IActionResult> RemoveComponent(Guid id)
        {
            (var componentRemoved, var category) = await cartService.RemoveComponent(id);
            if (componentRemoved)
            {
                ViewData[MessageConstant.SuccessMessage] = "Component was deleted";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Component was not deleted";
            }

            return Redirect($"/Home/{category}s");
        }

        public async Task<IActionResult> AddToCart(Guid id)
        {
            var user = await userManager.GetUserAsync(User);

            if (cartService.IsComponentInCart(user.Id, id.ToString()))
            {
                ViewData[MessageConstant.ErrorMessage] = "You already have component of this category";
            }

            (var componentAdded, var categoryName) = await cartService.AddToCart(user.Id, id.ToString());

            if (componentAdded)
            {
                ViewData[MessageConstant.SuccessMessage] = "Component added to cart";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "You already have component of this category";
            }

            return Redirect($"/Home/{categoryName}s");
        }

        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            var user = await userManager.GetUserAsync(User);

            (bool result, string categoryName) = await cartService.RemoveFromCart(user.Id, id.ToString());

            if (result)
            {
                ViewData[MessageConstant.SuccessMessage] = "Component removed Successfully";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Component was not removed";
            }

            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> ClearCart(string id)
        {
            var user = await userManager.GetUserAsync(User);
            var cart = await cartService.GetCartComponents(user.Id);

            if (await cartService.ClearCart(cart.CartId.ToString()))
            {
                ViewData[MessageConstant.SuccessMessage] = "Component removed Successfully";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Component was not removed";
            }

            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> ReplaceComponent(Guid id)
        {
            var user = await userManager.GetUserAsync(User);

            (bool result, string categoryName) = await cartService.RemoveFromCart(user.Id, id.ToString());

            if (result)
            {
                ViewData[MessageConstant.SuccessMessage] = "Component was replaced";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Component was not replaced";
            }

            return Redirect($"/Home/{categoryName}s");
        }
    }
}
