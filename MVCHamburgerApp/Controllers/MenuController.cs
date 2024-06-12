using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCHamburgerApp.Data;
using MVCHamburgerApp.Data.Entities;
using MVCHamburgerApp.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCHamburgerApp.Controllers
{
    [Authorize]
    public class MenuController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly HamburgerDbContext _context;

        public MenuController(HamburgerDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.ToListAsync();
            var extraToppings = await _context.ExtraToppings.ToListAsync();

            var model = new MenuViewModel
            {
                Menus = menus,
                ExtraToppings = extraToppings
            };

            return View(model);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> SiparisVer(OrderViewModel orderViewModel)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (orderViewModel.OrderItems == null || !orderViewModel.OrderItems.Any(item => item.Quantity > 0))
            {
                ModelState.AddModelError("", "Lütfen en az bir menü öğesi seçin ve miktarını belirtin.");
                return View("Index", new MenuViewModel { Menus = _context.Menus.ToList(), ExtraToppings = _context.ExtraToppings.ToList() }); //
            }

            var totalPrice = orderViewModel.OrderItems.Sum(item => item.ItemPrice);

            var order = new Order
            {
                AppUserId = appUser.Id,
                Date = DateTime.Now,
                TotalPrice = totalPrice
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var orderItem in orderViewModel.OrderItems.Where(item => item.Quantity > 0))  
            {
                if (orderItem.SelectedToppingIds != null && orderItem.SelectedToppingIds.Any())
                {
                    foreach (var toppingId in orderItem.SelectedToppingIds)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            MenuId = orderItem.MenuId,
                            Quantity = orderItem.Quantity,
                            ExtraToppingId = toppingId
                        };
                        _context.OrderDetails.Add(orderDetail);
                    }
                }
                else
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        MenuId = orderItem.MenuId,
                        Quantity = orderItem.Quantity,
                        ExtraToppingId = null 
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
            }
            await _context.SaveChangesAsync();

            var checkoutViewModel = new CheckoutViewModel
            {
                Order = orderViewModel,
                CustomerName = appUser.Name
            };

            TempData.Put("CheckoutViewModel", checkoutViewModel);
            TempData.Put("ExtraToppings", _context.ExtraToppings.ToList());

            return RedirectToAction("Checkout", "Order");
        }

    }
}
