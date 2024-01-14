using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Stomatologia.Models
{
    public class RegisterModel
    {
        [Required]
        public string Imie { get; set; }

        [Required]
        public string Nazwisko { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PESEL { get; set; }

        [Required]
        public string Adres { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        //public string Role { get;set; }
    }
}