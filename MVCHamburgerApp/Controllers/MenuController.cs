using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCHamburgerApp.Data;
using MVCHamburgerApp.Data.Entities;
using MVCHamburgerApp.Data.Enums;
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
                        case MenuSize.Orta:
                            sizePrice = 25;
                            break;
                        case MenuSize.Büyük:
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
            {//1 kere new orderDetail
                if (orderItem.SelectedToppingIds != null && orderItem.SelectedToppingIds.Any())
                {
                    foreach (var toppingId in orderItem.SelectedToppingIds)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            MenuId = orderItem.MenuId,
                            Quantity = orderItem.Quantity,
                            MenuSize = orderItem.Size, 
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

            TempData["LastOrderId"] = order.Id;

            return RedirectToAction("Checkout", "Order");

        }
        public async Task<IActionResult> EditOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Menu)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.ExtraTopping)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var model = new EditOrderViewModel
            {
                OrderId = order.Id,
                OrderItems = order.OrderDetails.Select(od => new OrderItemViewModel
                {
                    MenuId = od.MenuId,
                    MenuName = od.Menu.Name,
                    Quantity = od.Quantity,
                    Size=od.MenuSize,
                    SelectedToppingIds = od.ExtraTopping != null ? new List<int> { od.ExtraTopping.Id } : new List<int>(),
                    ItemPrice = od.Menu.BasePrice + (od.ExtraTopping?.Price ?? 0)
                }).ToList(),
                TotalPrice = order.TotalPrice
            };

            ViewBag.Menus = await _context.Menus.ToListAsync();
            ViewBag.ExtraToppings = await _context.ExtraToppings.ToListAsync();

            return View(model);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> EditOrder(EditOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Menus = await _context.Menus.ToListAsync();
                ViewBag.ExtraToppings = await _context.ExtraToppings.ToListAsync();
                return View(model);
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (order == null)
            {
                return NotFound();
            }

            _context.OrderDetails.RemoveRange(order.OrderDetails);
            _context.SaveChanges();

            foreach (var item in model.OrderItems)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.Id,
                        MenuId = item.MenuId,
                        Quantity = item.Quantity,
                        ExtraToppingId = item.SelectedToppingIds.FirstOrDefault()
                    });
                }
            }

            order.TotalPrice = model.OrderItems.Sum(item => item.ItemPrice);

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Checkout", "Order", new { orderId = order.Id });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            _context.OrderDetails.RemoveRange(order.OrderDetails);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }


}

