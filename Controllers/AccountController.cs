using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stomatologia.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;

namespace Stomatologia.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<IdentityUser> _logger;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender, ILogger<IdentityUser> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                        $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");

                    return RedirectToAction("UmowWizyte", "Account");
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Redirect("/Account/UmowWizyte");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string email, string passwordOld, string password)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Nieznalazony email.");
                    return View("Login");
                }

                var result = await _userManager.CheckPasswordAsync(user, passwordOld);
                if (result == true)
                {
                    await _userManager.ChangePasswordAsync(user, passwordOld, password);
                    return RedirectToAction("Login", "Account");
                }

                ModelState.AddModelError("", "Nieprawidłowe hasło.");
            }

            return View("Login");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {

                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Zresetuj hasło",
                   $"Zresetuj hasło, klikając <a href='{callbackUrl}'>tutaj</a>.");

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize]
        public IActionResult UmowWizyte()
        {
            var model = new UmowWizyteViewModel
            {
                DostepniStomatolodzy = PobierzDostepnychStomatologow()
            };

            return View(model);
        }

        public List<SelectListItem> PobierzDostepnychStomatologow()
        {
            var stomatolodzy = new List<Stomatolog>();

            // Tutaj dodaj swoich stomatologów do listy
            stomatolodzy.Add(new Stomatolog { Id = 1, Imie = "Jan", Nazwisko = "Kowalski", Specjalizacja = "Specjalizacja 1" });
            stomatolodzy.Add(new Stomatolog { Id = 2, Imie = "Anna", Nazwisko = "Nowak", Specjalizacja = "Specjalizacja 2" });

            var selectListItems = stomatolodzy.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.ImieNazwisko
            });

            return selectListItems.ToList();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

    }
}