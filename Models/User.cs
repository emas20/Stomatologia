using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Stomatologia.Models
{
    public class User : IdentityUser
    {
        // Dodaj niestandardowe właściwości użytkownika, jeśli są potrzebne
        public int UserId { get; set; }
        // [Key]   
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public override string Email { get; set; }
        public string Password { get; set; }
        public string PESEL { get; set; }
        public override string PhoneNumber { get; set; }
       // public DateTime DataUrodzenia { get; set; }
        public string Adres { get; set; }
        // Dodaj inne pola użytkownika, jeśli są wymagane
        public User()
        {
            // Konstruktor
            Email = "Email";
            Imie = "Imie";
            Nazwisko= "Nazwisko";
            Password = "Hasło";
            PESEL = "PESEL";
            PhoneNumber = "Numer Telefonu";
            Adres = "Adres";
            // Dodaj tutaj ewentualną logikę inicjalizacji, jeśli jest potrzebna
        }

        // public User(string firstName, string lastName, string email, string password, string PESEL, string phoneNumber)
        // {
        //  FirstName = firstName;
        //  LastName = lastName;
        //  Email = email;
        //  Password = password;
        //  this.PESEL = PESEL;
        //  PhoneNumber = phoneNumber;
        // }
    }
}