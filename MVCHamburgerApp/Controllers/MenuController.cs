using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCHamburgerApp.Data;
using MVCHamburgerApp.Models;
using System.Threading.Tasks;

namespace MVCHamburgerApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly HamburgerDbContext _context;

        public MenuController(HamburgerDbContext context)
        {
            _context = context;
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
    }
}
