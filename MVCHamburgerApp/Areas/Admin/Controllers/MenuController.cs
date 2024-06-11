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
    [Authorize(Roles = "Admin")]

    public class MenuController : Controller
    {
        private readonly HamburgerDbContext _context;

        public MenuController(HamburgerDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Menu
        public async Task<IActionResult> Index()
        {
            return View(await _context.Menus.ToListAsync());
        }

        // GET: Admin/Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Dosyayı kaydetme işlemi
                    if (menu.PictureFile != null)
                    {
                        var fileName = Path.GetFileName(menu.PictureFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await menu.PictureFile.CopyToAsync(fileStream);
                        }

                        menu.PictureName = fileName; // Kaydedilen dosya adını PictureName'e atamak için
                    }

                    _context.Add(menu);
                    await _context.SaveChangesAsync();
                    ViewData["SuccessMessage"] = "Basariyla eklenmistir.";
                    return View();
                }
                catch (Exception ex)
                {
                    // Hata durumunda hata mesajını ayarlayın
                    ViewData["ErrorMessage"] = "Hata olustu: " + ex.Message;
                    return View(menu);
                }
            }
            else
            {
                // ModelState hatalarını yakalayın ve hata mesajlarını ayarlayın
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                string errorMessages = string.Join("; ", errors.Select(e => e.ErrorMessage));
                ViewData["ErrorMessage"] = "Gecersiz veri girisi: " + errorMessages;
            }

            return View(menu);
        }



        // GET: Admin/Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            // PictureFile alanını set etmeden önce PictureName'i kontrol et
            if (!string.IsNullOrEmpty(menu.PictureName))
            {
                var fileName = Path.Combine("/images", menu.PictureName); // resim dosyasının yolunu al
                menu.PictureFile = new FormFile(null, 0, 0, null, fileName); // PictureFile'ı set et
            }

            return View(menu);
        }

        // POST: Admin/Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,BasePrice,PictureName,PictureFile,Size")] Menu menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingMenu = await _context.Menus.FindAsync(id);

                if (existingMenu == null)
                {
                    return NotFound();
                }

                if (menu.PictureFile != null && menu.PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(menu.PictureFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await menu.PictureFile.CopyToAsync(fileStream);
                    }

                    existingMenu.PictureName = fileName;
                }

                existingMenu.Name = menu.Name;
                existingMenu.Description = menu.Description;
                existingMenu.BasePrice = menu.BasePrice;
                existingMenu.Size = menu.Size;

                try
                {
                    _context.Update(existingMenu);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Başarılı bir şekilde güncellenmiştir.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Hata durumunda ModelState hatasını görüntüle
            var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["ErrorMessage"] = "Hata oluştu. " + string.Join(" ", errorMessages);
            return View(menu);
        }



        // GET: Admin/Menu/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Menu ID is required to perform delete operation.");
            }

            var menu = _context.Menus.FirstOrDefault(m => m.Id == id);
            if (menu == null)
            {
                return NotFound("Menu not found.");
            }

            return View(menu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }
    }
}
