using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stomatologia.Data;
using Stomatologia.Models;

namespace Stomatologia.Services
{
    public class StomatologService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<StomatologService> _logger;
        public StomatologService(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext, ILogger<StomatologService> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<bool> CreateStomatologAsync(string userName, string password)
        {
            var stomatolog = new Stomatolog { UserName = userName };

            var result = await _userManager.CreateAsync(stomatolog, password);

            return result.Succeeded;
        }
        public List<Stomatolog> GetAvailableStomatologists()
        {
            // Pobierz listę stomatologów z bazy danych
            return _dbContext.Stomatolodzy.ToList();
        }

        public List<DateTime> GetAvailableDates()
        {
            var availableDates = _dbContext.Wizyty.Select(w => w.WybranaData.Date).Distinct().ToList();

            Console.WriteLine($"Liczba dostępnych dat w GetAvailableDates: {availableDates.Count}");

            return availableDates;
        }

        public List<string> GetAvailableHours(DateTime selectedDate, string stomatologId)
        {
            // Pobierz już umówione godziny na wybraną datę i stomatologa
            var umowioneGodziny = _dbContext.Wizyty
                .Where(w => w.WybranyStomatologId == stomatologId && w.WybranaData.Date == selectedDate.Date)
                .Select(w => w.WybranaGodzina)
                .ToList();
            // Tutaj pobieramy godziny dostępne dla wybranej daty
            // Załóżmy, że stomatolog pracuje od 8:00 do 17:00, a spotkanie trwa godzinę
            var godzinyPracy = Enumerable.Range(8, 10).Select(hour => $"{hour}:00").ToList();

            // Odfiltruj dostępne godziny, usuwając już umówione
            var dostepneGodziny = godzinyPracy.Except(umowioneGodziny).ToList();
            Console.WriteLine($"Liczba dostępnych godzin w GetAvailableHours: {dostepneGodziny.Count}");

            return dostepneGodziny;
        }
    

    public async Task<bool> AuthenticateAsync(string userName, string password)
        {
            var stomatolog = await _userManager.FindByNameAsync(userName);

            if (stomatolog != null)
            {
                var result = await _userManager.CheckPasswordAsync(stomatolog, password);

                // Uwierzytelnienie udane
                return result;
            }

            // Nieprawidłowe dane logowania
            return false;
        }
    }
}