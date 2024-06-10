using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCHamburgerApp.Data;
using MVCHamburgerApp.Data.Entities;

namespace MVCHamburgerApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ExtraToppingController : Controller
    {
        private readonly HamburgerDbContext _context;

        public ExtraToppingController(HamburgerDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ExtraTopping
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExtraToppings.ToListAsync());
        }

        // GET: Admin/ExtraTopping/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extraTopping = await _context.ExtraToppings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extraTopping == null)
            {
                return NotFound();
            }

            return View(extraTopping);
        }

        // GET: Admin/ExtraTopping/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ExtraTopping/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] ExtraTopping extraTopping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(extraTopping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(extraTopping);
        }

        // GET: Admin/ExtraTopping/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extraTopping = await _context.ExtraToppings.FindAsync(id);
            if (extraTopping == null)
            {
                return NotFound();
            }
            return View(extraTopping);
        }

        // POST: Admin/ExtraTopping/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] ExtraTopping extraTopping)
        {
            if (id != extraTopping.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(extraTopping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExtraToppingExists(extraTopping.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(extraTopping);
        }

        // GET: Admin/ExtraTopping/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extraTopping = await _context.ExtraToppings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extraTopping == null)
            {
                return NotFound();
            }

            return View(extraTopping);
        }

        // POST: Admin/ExtraTopping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var extraTopping = await _context.ExtraToppings.FindAsync(id);
            if (extraTopping != null)
            {
                _context.ExtraToppings.Remove(extraTopping);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExtraToppingExists(int id)
        {
            return _context.ExtraToppings.Any(e => e.Id == id);
        }
    }
}
