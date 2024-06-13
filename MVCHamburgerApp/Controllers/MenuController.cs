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

            if (!orderViewModel.OrderItems.Any(item => item.Quantity > 0))
            {
                ModelState.AddModelError("", "Lütfen en az bir menü öğesi seçin ve miktarını belirtin.");
                var model = new MenuViewModel
                {
                    Menus = await _context.Menus.ToListAsync(),
                    ExtraToppings = await _context.ExtraToppings.ToListAsync()
                };
                return View("Index", model); 
            }

            CheckoutViewModel checkoutViewModel = null; 


            foreach (var orderItem in orderViewModel.OrderItems.Where(item => item.Quantity > 0))
            {
                var menu = _context.Menus.Find(orderItem.MenuId); 

                if (menu != null)
                {
                    decimal sizePrice = 0;
                    switch (orderItem.Size)
                    {
                        case "Orta":
                            sizePrice = 25;
                            break;
                        case "Büyük":
                            sizePrice = 50;
                            break;
                    }

                    decimal toppingPrice = 0;
                    if (orderItem.SelectedToppingIds != null)
                    {
                        toppingPrice = _context.ExtraToppings
                            .Where(t => orderItem.SelectedToppingIds.Contains(t.Id))
                            .Sum(t => t.Price);
                    }

                    orderItem.ItemPrice = (menu.BasePrice + sizePrice + toppingPrice) * orderItem.Quantity;
                }
            }

            orderViewModel.TotalPrice = orderViewModel.OrderItems
                .Where(item => item.Quantity > 0)
                .Sum(item => item.ItemPrice);

            var order = new Order
            {
                AppUserId = appUser.Id,
                Date = DateTime.Now,
                TotalPrice = orderViewModel.TotalPrice 
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


            checkoutViewModel = new CheckoutViewModel
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
