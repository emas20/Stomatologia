using Microsoft.AspNetCore.Identity;
namespace Stomatologia.Models
{
    public class Stomatolog : IdentityUser
    {
        //public int Id { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }

        public string? Specjalizacja { get; set; }
        public string ImieNazwisko
        {
            get { return Imie + " " + Nazwisko; }
        }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<UmowWizyteViewModel>? Wizyty { get; set; }
    }
}