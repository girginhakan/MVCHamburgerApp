using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCHamburgerApp.Areas.Admin.Models;
using MVCHamburgerApp.Data;
using MVCHamburgerApp.Data.Entities;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Create(MenuViewModel menuViewModel)
        {
            if (menuViewModel.BasePrice < 0)
            {
                TempData["Hata"] = "Fiyat negatif olamaz";
                return View(menuViewModel);
            }

            Menu menu = new Menu
            {
                Name = menuViewModel.Name,
                BasePrice = menuViewModel.BasePrice,
                Description = menuViewModel.Description,
                Size = menuViewModel.Size
            };

            if (menuViewModel.Picture != null)
            {
                // Dosyanın adını, urun nesnesinin resim adına ata
                menu.PictureName = menuViewModel.Picture.FileName;

                // Dosyanın kaydedileceği konumu belirle
                var konum = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", menu.PictureName);

                // Kaydetmek için bir akış ortamı oluştur
                using (var akisOrtami = new FileStream(konum, FileMode.Create))
                {
                    // Resmi kaydet
                     menuViewModel.Picture.CopyToAsync(akisOrtami);
                }
            }

            _context.Menus.Add(menu);
             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Menu/Edit/5
        public IActionResult Edit(int? id)
        {
            var guncellenecekUrun = _context.Menus.Find(id);
            MenuViewModel model = new MenuViewModel();
            model.Name = guncellenecekUrun.Name;
            model.Description = guncellenecekUrun.Description;
            model.BasePrice = guncellenecekUrun.BasePrice;
            ViewBag.ResimAdi = guncellenecekUrun.PictureName;
            TempData["Id"] = id;

            return View(model);
        }

        // POST: Admin/Menu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,BasePrice,PictureName,PictureFile,Size")] MenuViewModel model)
        {


            if (ModelState.IsValid)
            {
                var guncellenenMenu = await _context.Menus.FindAsync(id);

                if (guncellenenMenu == null)
                {
                    return NotFound();
                }
                guncellenenMenu.Name = model.Name;
                guncellenenMenu.Description = model.Description;
                guncellenenMenu.BasePrice = model.BasePrice;
                guncellenenMenu.Size = model.Size;

                if (model.Picture != null && model.Picture.FileName != guncellenenMenu.PictureName)
                {
                    var fileName = Path.GetFileName(model.Picture.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    if (guncellenenMenu.PictureName is not null)
                    {
                        DeletePicture(guncellenenMenu);
                    }
                    guncellenenMenu.PictureName = fileName;
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Picture.CopyToAsync(fileStream);
                    }

                }
                _context.Update(guncellenenMenu);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Başarılı bir şekilde güncellenmiştir.";
                return RedirectToAction(nameof(Index));


            }

            // Hata durumunda hata mesajını ModelState hatasıyla beraber göster
            var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["ErrorMessage"] = "Hata oluştu. " + string.Join(" ", errorMessages);
            return View(model);
        }


        public void DeletePicture(Menu menu)
        {
            var resmiKullananBaskaUrunVarMi = _context.Menus.Any(u => u.PictureName == menu.PictureName && u.Id != menu.Id);

                string deletedPicture = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", menu.PictureName);
                System.IO.File.Delete(deletedPicture);

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
        public async Task<IActionResult> DeleteConfirmed(Menu menu)
        {
            //var menu = await _context.Menus.FindAsync(id);
            if (menu.PictureName is not null)
            {
                DeletePicture(menu);
            }
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }
    }
}
