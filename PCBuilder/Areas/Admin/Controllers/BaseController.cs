using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Core.Constants;

namespace PCBuilder.Areas.Admin.Controllers
{
    [Authorize(Roles = UserConstants.Roles.Administrator)]
    [Area("Admin")]
    public class BaseController : Controller
    {
    }
}
