using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Core.Contracts;
using PCBuilder.Infrastructure.Data.Identity;
using PCBuilder.Models;
using System.Diagnostics;

namespace PCBuilder.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICartService cartService;
        private readonly IComputerService computerService;

        public HomeController(ILogger<HomeController> _logger, ICartService _cartService, UserManager<ApplicationUser> _userManager, IComputerService _computerService)
        {
            logger = _logger;
            cartService = _cartService;
            userManager = _userManager;
            computerService = _computerService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    if (!User.IsInRole("Guest"))
                    {
                        await userManager.AddToRoleAsync(user, "Guest");
                    }
                }

                var cart = await cartService.GetCartComponents(user.Id);
                if (cart != null)
                {
                    ViewBag.ViewModel = cart;
                }

                if (cart.Components == null || cart.Components.Count <= 0)
                {
                    return View();
                }

                return View(cart);
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Computers()
        {
            var user = await userManager.GetUserAsync(User);
            var computers = await computerService.GetUserComputers(user.Id.ToString());

            if (user != null)
            {
                var cart = await cartService.GetCartComponents(user.Id);
                ViewBag.ViewModel = cart;
            }

            return View(computers);
        }

        [Authorize]
        public async Task<IActionResult> BuildComputer()
        {
            var user = await userManager.GetUserAsync(User);
            var cart = await cartService.GetCartComponents(user.Id.ToString());

            if (cart.Components.Count < 7)
            {
                return Redirect("/Cart/Cart");
            }

            if (await computerService.BuildComputer(cart))
            {
                await cartService.ClearCart(cart.CartId);
            }

            return RedirectToAction("Computers");
        }

        [Authorize]
        public async Task<IActionResult> DetailsComputer(string id)
        {
            var computer = await computerService.GetComputer(id);

            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);
                var cart = await cartService.GetCartComponents(user.Id);
                ViewBag.ViewModel = cart;
            }

            return View(computer);
        }

        public async Task<IActionResult> GetAllComponents(string category)
        {
            var components = await cartService.GetAllComponents(category);

            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);
                var cart = await cartService.GetCartComponents(user.Id);
                ViewBag.ViewModel = cart;
            }

            if (components != null && components.Count > 0)
            {
                return View("AllComponents", components);
            }

            return View("ErrorCustom", $"The {category} category is empty at the moment.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ErrorCustom()
        {
            return View();
        }
    }
}