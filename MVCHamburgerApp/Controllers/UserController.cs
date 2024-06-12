using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using MVCHamburgerApp.Data.Entities;
using MVCHamburgerApp.Models;


namespace MVCHamburgerApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<AppUser> signInManager, ICompositeViewEngine viewEngine, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _viewEngine = viewEngine;
            _configuration = configuration;
        }


        [HttpGet]
        public IActionResult Register()
        {
            UserViewModel model = new UserViewModel();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
                return View(model);


            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Şifre ile şifre tekrarı aynı değildir.";
                return View(model);
            }


            var users = _userManager.Users.ToList();

            if (_userManager.Users.Any(u=>u.Email==model.Email))
            {
                ViewBag.ErrorMessage = $"{model.Email} adresi zaten kayıtlıdır";
                return View(model);
            }
            //foreach (var item in users)
            //{
            //    if (model.Email == item.Email)
            //    {
                   
            //    }
            //}

            var hasher = new PasswordHasher<AppUser>();

            AppUser user = new AppUser
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                EmailConfirmed = true,
                NormalizedEmail = model.Email.ToUpper(),
                PasswordHash = hasher.HashPassword(null, model.Password),
                UserName = model.Name,
                NormalizedUserName = model.Name.ToUpper()
            };

            IdentityResult result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");


                TempData["Message"] = "Kayıt başarılı! Giriş yapabilirsiniz.";

                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Kayıt başarısız, lütfen tüm alanları eksiksiz ve doğru bir şekilde doldurunuz";
                return View(model);
            }


        }


        [HttpGet]
        public IActionResult Login()
        {
            Models.LoginViewModel model = new Models.LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Lütfen kutucukları eksiksiz ve doğru doldurunuz";
                return View(model);
            }


            AppUser user = _userManager.FindByEmailAsync(model.Email).Result;

            if (user is null)
            {
                ViewBag.ErrorMessage = "Bu e-mail ile kullanıcı bulunamamıştır";
                return View(model);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;

            if (!signInResult.Succeeded)
            {
                ViewBag.ErrorMessage = "Şifre yanlıştır.";
                return View(model);
            }

            ViewBag.LoginSuccess = user.Name;

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }




    }
}
