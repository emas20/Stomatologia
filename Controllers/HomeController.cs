using Microsoft.AspNetCore.Mvc;
using Stomatologia.Models;
using System.Diagnostics;

namespace Stomatologia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var clinic = new Clinic
            {
                Name = "Klinika Dentystyczna Ząbek",
                Address = "Os. Wichrowe Wzgórze 241/121, 60-281 Poznań",
                PhoneNumber = "+48 61 251 322"
            };

            return View(clinic);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}