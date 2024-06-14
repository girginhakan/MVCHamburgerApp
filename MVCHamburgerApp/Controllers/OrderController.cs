using Microsoft.AspNetCore.Mvc;
using MVCHamburgerApp.Models;

namespace MVCHamburgerApp.Controllers
{
    public class OrderController : Controller
    {
       
        public IActionResult Checkout()
        {
            var checkoutViewModel = TempData.Get<CheckoutViewModel>("CheckoutViewModel"); 

            return View(checkoutViewModel);
        }
        public IActionResult CompletedOrder()
        {
            return View();
        }
    }
}
