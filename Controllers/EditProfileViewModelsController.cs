using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stomatologia.Data;
using Stomatologia.Models;

namespace Stomatologia.Controllers
{
    public class EditProfileViewModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EditProfileViewModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EditProfileViewModels
        public async Task<IActionResult> Index()
        {
              return _context.EditProfileViewModel != null ? 
                          View(await _context.EditProfileViewModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.EditProfileViewModel'  is null.");
        }

        // GET: EditProfileViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EditProfileViewModel == null)
            {
                return NotFound();
            }

            var editProfileViewModel = await _context.EditProfileViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editProfileViewModel == null)
            {
                return NotFound();
            }

            return View(editProfileViewModel);
        }

        // GET: EditProfileViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EditProfileViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,Email,PhoneNumber,CurrentPassword,NewPassword,ConfirmNewPassword")] EditProfileViewModel editProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editProfileViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editProfileViewModel);
        }

        // GET: EditProfileViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EditProfileViewModel == null)
            {
                return NotFound();
            }

            var editProfileViewModel = await _context.EditProfileViewModel.FindAsync(id);
            if (editProfileViewModel == null)
            {
                return NotFound();
            }
            return View(editProfileViewModel);
        }

        // POST: EditProfileViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,Email,PhoneNumber,CurrentPassword,NewPassword,ConfirmNewPassword")] EditProfileViewModel editProfileViewModel)
        {
            if (id != editProfileViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editProfileViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditProfileViewModelExists(editProfileViewModel.Id))
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
            return View(editProfileViewModel);
        }

        // GET: EditProfileViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EditProfileViewModel == null)
            {
                return NotFound();
            }

            var editProfileViewModel = await _context.EditProfileViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editProfileViewModel == null)
            {
                return NotFound();
            }

            return View(editProfileViewModel);
        }

        // POST: EditProfileViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EditProfileViewModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EditProfileViewModel'  is null.");
            }
            var editProfileViewModel = await _context.EditProfileViewModel.FindAsync(id);
            if (editProfileViewModel != null)
            {
                _context.EditProfileViewModel.Remove(editProfileViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditProfileViewModelExists(int id)
        {
          return (_context.EditProfileViewModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
