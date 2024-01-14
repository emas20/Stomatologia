using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stomatologia.Data;
using Stomatologia.Models;

namespace Stomatologia.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsersController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Profil()
        {
            // Pobierz zalogowanego użytkownika
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Obsługa błędu, użytkownik niezalogowany
                return RedirectToAction("Login");
            }

            // Przekazanie modelu użytkownika do widoku
            return View(user);
        }

        public IActionResult UmawianieWizyt()
        {
            // Logika umawiania wizyt, np. pobieranie dostępnych terminów
            // i wyświetlanie ich w widoku
            return View();
        }

        //public IActionResult HistoriaWizyt()
        //{
            // Pobierz historię wizyt użytkownika z bazy danych
         //   var userId = _userManager.GetUserId(User);
            //var historiaWizyt = _context.HistoriaWizyt.Where(w => w.UserId == userId).ToList();

            // Przekazanie historii wizyt do widoku
          //return View(historiaWizyt);
       // }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*
         * 
         * public async Task<IActionResult Rejestracja(RejestracjaViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Utwórz nowego użytkownika na podstawie danych z formularza
            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                // Inne właściwości użytkownika
            };

            var result = await _userManager.CreateAsync(newUser, model.Haslo);

            if (result.Succeeded)
            {
                // Zaloguj nowego użytkownika
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Profil");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

         * */

        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Email,Password,PESEL,PhoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Email,Password,PESEL,PhoneNumber")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
