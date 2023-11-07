using System.ComponentModel.DataAnnotations;

namespace Stomatologia.Models
{
    public class User
    {
      
            [Key]
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PESEL { get; set; }
            public string PhoneNumber { get; set; }
        // Dodaj inne pola użytkownika, jeśli są wymagane
        public User()
        {
            // Konstruktor
            Email = "Email";
            FirstName = "Imie";
            LastName = "Nazwisko";
            Password = "Hasło";
            PESEL = "PESEL";
            PhoneNumber = "Numer Telefonu";
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

