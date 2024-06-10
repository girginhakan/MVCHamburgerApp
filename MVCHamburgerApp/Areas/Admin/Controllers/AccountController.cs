using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCHamburgerApp.Areas.Admin.Models;
using MVCHamburgerApp.Data;
using MVCHamburgerApp.Data.Entities;

namespace MVCHamburgerApp.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {

                ModelState.AddModelError(string.Empty, "E-mail adresi geçersizdir");
                return View(model);
            }


            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false,true);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home", new { area = "Admin" });


            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
