using Microsoft.AspNetCore.Identity;
using System;
using Stomatologia.Controllers;

namespace Stomatologia.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? Pesel { get; set; }
        public override string? Email { get; set; }
        public string? Adres { get; set; }
        public string? role { get; set; }
    }
}

