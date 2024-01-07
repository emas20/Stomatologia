using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stomatologia.Models;

namespace Stomatologia.Controllers
{
    public class UmowWizyteController : Controller
    {
        private readonly IEmailSender _emailSender;

        public UmowWizyteController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

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
    }
}