using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Core.Constants;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models.Cart;
using PCBuilder.Core.Models.Home;
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

        public PartialViewResult Action()
        {
            var user = userManager.GetUserAsync(User);
            var model = cartService.GetCartComponents(user.Id.ToString());

            return PartialView("~/Views/Shared/_SidebarContent.cshtml", model);
        }

        public async Task<IActionResult> Index()
        {
            //var user = await userManager.GetUserAsync(User);
            //var cart = await cartService.GetCartComponents(user.Id);

            //ViewBag.ViewModel = cart;

            //if (cart.Components == null || cart.Components.Count <= 0)
            //{
            //    return View();
            //}

            return View();
        }

        public async Task<IActionResult> Computers()
        {
            var user = userManager.GetUserAsync(User).Result;
            var computers = computerService.GetUserComputers(user.Id.ToString()).Result;

            return View(computers);
        }

        public async Task<IActionResult> BuildComputer()
        {
            var user = userManager.GetUserAsync(User).Result;
            var cart = cartService.GetCartComponents(user.Id.ToString()).Result;

            if (cart.Components.Count < 7)
            {
                return Redirect("/Cart/Cart");
            }

            if (await computerService.BuildComputer(cart))
            {
                await cartService.ClearCart(cart.CartId.ToString());
            }

            return RedirectToAction("Computers");
        }

        public async Task<IActionResult> DetailsComputer(string id)
        {
            var computer = await computerService.GetComputer(id);

            return View(computer);
        }

        public async Task<IActionResult> GetAllComponents(string category)
        {
            var components = await cartService.GetAllComponents(category);

            if (components != null && components.Count > 0)
            {
                return View("AllComponents", components);
            }

            return View("ErrorCustom");
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