using Microsoft.AspNetCore.Mvc;

namespace MVCHamburgerApp.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
