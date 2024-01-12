using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stomatologia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Stomatologia.Data;

namespace Stomatologia.Controllers
{
    public class UmowWizyteController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public UmowWizyteController(IEmailSender emailSender, ApplicationDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }
    }
       
        /*public IActionResult UmowWizyte()
        {

            var dostepniStomatolodzy = _context.Stomatolodzy
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.ImieNazwisko
                }).ToList();
        */
        /*var model = new UmowWizyteViewModel
        {
            DostepniStomatolodzy = dostepniStomatolodzy,
            WybranaData = DateTime.Now // Domyślnie ustawiamy na dzisiejszą datę

        };

        return View(model);
    }
        */
        /*public IActionResult UmowWizyte()


        {
            var model = new UmowWizyteViewModel
            {
                DostepniStomatolodzy = PobierzDostepnychStomatologow(),
                WybranaData = DateTime.Now // Domyślnie ustawiamy na dzisiejszą datę
            };

            return View(model);
        }
        */

        /* public List<SelectListItem> PobierzDostepnychStomatologow()
         {
             var stomatolodzy = new List<Stomatolog>();

             // Tutaj dodajemy swoich stomatologów do listy
             stomatolodzy.Add(new Stomatolog { Id = 1, Imie = "Jan", Nazwisko = "Kowalski", Specjalizacja = "Specjalizacja 1" });
             stomatolodzy.Add(new Stomatolog { Id = 2, Imie = "Anna", Nazwisko = "Nowak", Specjalizacja = "Specjalizacja 2" });

             var selectListItems = stomatolodzy.Select(s => new SelectListItem
             {
                 Value = s.Id.ToString(),
                 Text = s.ImieNazwisko
             });

             return selectListItems.ToList();
         }
        */

}
