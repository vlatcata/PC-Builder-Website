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

            return Redirect("/Home/Index");
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

            return Redirect("/Home/Index");
        }

        [Authorize(Roles = UserConstants.Roles.Administrator)]
        public async Task<IActionResult> RemoveComponent(Guid id)
        {
            if (await cartService.RemoveComponent(id))
            {
                ViewData[MessageConstant.SuccessMessage] = "Component was deleted";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Component was not deleted";
            }

            return Redirect("/Home/Index");
        }

        public async Task<IActionResult> AddToCart(Guid id)
        {
            var user = await userManager.GetUserAsync(User);

            if (cartService.IsComponentInCart(user.Id, id.ToString()))
            {
                ViewData[MessageConstant.ErrorMessage] = "You already have component of this category";
            }
            
            if(await cartService.AddToCart(user.Id, id.ToString()))
            {
                ViewData[MessageConstant.SuccessMessage] = "Component added to cart";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "You already have component of this category";
            }

            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            var user = await userManager.GetUserAsync(User);

            if (await cartService.RemoveFromCart(user.Id, id.ToString()))
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

        public IActionResult ReplaceComponent(string id)
        {
            return View();
        }
    }
}
