using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Stomatologia.Models
{
    public class UserProfileViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string? Imie { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string? Nazwisko { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
        public string?  Email { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string? Pesel { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string? Adres { get; set; }

        [Display(Name = "Aktualne hasło")]
        public string? CurrentPassword { get; set; }

        [Display(Name = "Nowe hasło")]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasło i jego potwierdzenie nie pasują do siebie.")]
        public string? ConfirmNewPassword { get; set; }
        public string? Role { get; set; }

        // Pole przechowujące dane użytkownika
        public IdentityUser? IdentityUser { get; set; }
    }
}
