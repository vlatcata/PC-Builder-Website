using Microsoft.AspNetCore.Mvc;

namespace PCBuilder.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
