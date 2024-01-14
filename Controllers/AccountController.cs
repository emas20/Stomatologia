using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stomatologia.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Stomatologia.Interfaces;
using Stomatologia.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace Stomatologia.Controllers
{
    public class AccountController : Controller
    {
       //private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<IdentityUser> _logger;
        private readonly IWizytyService _wizytyService;
        private readonly StomatologService _stomatologService;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<IdentityUser> logger, IWizytyService wizytyService, StomatologService stomatologService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
            _logger = logger;
            _wizytyService = wizytyService;
            _stomatologService = stomatologService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                //var result = await _userManager.CreateAsync(user, model.Password);
                
                var claims = new List<Claim>
     {
         new Claim("Imie", model.Imie),
         new Claim("Nazwisko", model.Nazwisko),
         new Claim("Pesel", model.PESEL),
         new Claim("Telefon", model.PhoneNumber),
         new Claim("Adres", model.Adres),
         new Claim("Email",model.Email ),
         //new Claim("Role", model.Role="User")

     };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    
                    // Dodawanie claimów
                    await _userManager.AddClaimsAsync(user, claims);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    
                    // Dodawanie do roli
                    await _userManager.AddToRoleAsync(user, "User");
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    // $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    

                    return RedirectToAction("UmowWizyte", "Account");
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        ModelState.AddModelError("", "Błąd Rejestracji konta");
                    }
                }
            }

            return View(model);
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Redirect("/Account/UmowWizyte");
                }

                ModelState.AddModelError("", "Nieprawidłowa próba logowania");
            }

            return View(model);
        }
        [Authorize]
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

        [Authorize]
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
                _logger.LogInformation("Zresetuj hasło","Zresetuj hasło, klikając <a href='{callbackUrl}'>tutaj</a>.");

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAvailableDatesAndHours(string stomatologId, DateTime selectedDate)
        {
            _logger.LogInformation($"Pobieranie dostępnych dat i godzin dla stomatologa o ID {stomatologId} w dniu {selectedDate}.");
            var availableDates = _stomatologService.GetAvailableDates();
            var availableHours = _stomatologService.GetAvailableHours(selectedDate, stomatologId);

            _logger.LogInformation($"Dostępne daty: {string.Join(", ", availableDates)}");
            _logger.LogInformation($"Dostępne godziny: {string.Join(", ", availableHours)}");

            return Json(new { AvailableDates = availableDates, AvailableHours = availableHours });
        }

        [Authorize]
        [HttpGet]
        
        public IActionResult UmowWizyte(string selectedStomatologId)
        {
            _logger.LogInformation($"Umawianie wizyty dla stomatologa o ID {selectedStomatologId}.");

            var dostepneDaty = _stomatologService.GetAvailableDates();

            // Pobierz pierwszą datę jako domyślną
            var defaultDate = dostepneDaty.FirstOrDefault();

            var dostepniStomatolodzy = _stomatologService.GetAvailableStomatologists();
            ViewBag.AvailableStomatologists = dostepniStomatolodzy;

            //var availableStomatologists = _stomatologService.GetAvailableStomatologists();
           // ViewBag.AvailableStomatologists = availableStomatologists;

            var availableHours = defaultDate != default(DateTime)
                ? _stomatologService.GetAvailableHours(defaultDate, selectedStomatologId)
                : new List<string>(); 
            ViewBag.AvailableDates = dostepneDaty;
            ViewBag.AvailableHours = availableHours;

            // Dodaj debug
            Console.WriteLine($"Liczba stomatologów w ViewBag: {dostepniStomatolodzy.Count}");
            Console.WriteLine($"Liczba dostępnych dat w ViewBag: {dostepneDaty.Count}");
            Console.WriteLine($"Liczba dostępnych godzin w ViewBag: {availableHours?.Count ?? 0}");

            return View();
        }
            
            
        [Authorize]
       
        public IActionResult PotwierdzWizyte(UmowWizyteViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tutaj można wykorzystać serwis do zapisania informacji o wizycie
                // np. zapis do bazy danych

                var wizyta = new UmowWizyteViewModel
                {
                    WybranyStomatologId = model.WybranyStomatologId,
                    WybranaData = model.WybranaData,
                    WybranaGodzina = model.WybranaGodzina
                };
                _wizytyService.ZapiszWizyte(wizyta);
                TempData["SuccessMessage"] = "Wizyta została pomyślnie zapisana.";
                HttpContext.Session.Remove("WybranaData");
                HttpContext.Session.Remove("WybranaGodzina");
                return RedirectToAction("Index", "Home");
            }
            return View(model);
    }
        [Authorize]
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            int pageSize = 10;
            var data = await _userManager.Users.ToListAsync();
            var users = data.OrderBy(u => u.Id).ToPagedListAsync(pageIndex, pageSize);

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                // Sprawdź, czy użytkownik edytuje swoje własne dane
                if (user.Id != model.Id.ToString())
                {
                    return Forbid();
                }

                // Zaktualizuj dane użytkownika
               // user.Imie = model.Imie;
               // user.Nazwisko = model.Nazwisko;
               // user.Email = model.Email;
              //  user.Pesel = model.Pesel;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Użytkownik zaktualizował swoje dane.");
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Profil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var imieClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Imie");
           
            var model = new UserProfileViewModel
            {
                //Id = (string)user.Id,
                //Id = user.Id,
                Imie = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Imie")?.Value,
                Nazwisko = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Nazwisko")?.Value,
                Pesel = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Pesel")?.Value,
                PhoneNumber = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Telefon")?.Value,
                Adres = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Adres")?.Value,
                Email = user.Email   
            };

            return View(model);
            
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

    }
}