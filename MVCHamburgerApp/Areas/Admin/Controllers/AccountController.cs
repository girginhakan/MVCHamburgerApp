using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCHamburgerApp.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AccountController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }
    }
}
