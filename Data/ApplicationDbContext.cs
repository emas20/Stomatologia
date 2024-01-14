using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stomatologia.Models;
using System;
using System.Linq;

namespace Stomatologia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Stomatolog> Stomatolodzy { get; set; }
        public DbSet<UmowWizyteViewModel> Wizyty { get; set; }

        public DbSet<Stomatologia.Models.User>? User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UmowWizyteViewModel>().HasKey(w => w.Id);

            modelBuilder.Entity<UmowWizyteViewModel>()
                        .HasOne(w => w.Stomatolog)
                        .WithMany(s => s.Wizyty)
                        .HasForeignKey(w => w.WybranyStomatologId);
        }

        public static class ApplicationDbContextSeed
        {
            public static void Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager)
            {
                if (!context.Stomatolodzy.Any())
                {
                    var stomatolodzy = new Stomatolog[]
                    {
                new Stomatolog {Id = "3", Imie = "Krzysztof", Nazwisko = "Nowak", Specjalizacja = "Stomatolog", UserName = "KNowak@clinic.pl" },
                new Stomatolog {Id = "4", Imie = "Monika", Nazwisko = "Kowalska", Specjalizacja = "Stomatolog Dziecięcy", UserName = "MKowalska@clinic.pl" },
                new Stomatolog {Id = "5", Imie = "Katarzyna", Nazwisko = "Grabowska", Specjalizacja = "Periodontolog", UserName = "KGrabowska@clinic.pl" },
                new Stomatolog {Id = "6", Imie = "Piotr", Nazwisko = "Wawrzyniak", Specjalizacja = "Protetyk", UserName = "PWawrzyniak@clinic.pl" },
                new Stomatolog {Id = "7", Imie = "Adam", Nazwisko = "Leon", Specjalizacja = "Ortodonta", UserName = "ALeon@clinic.pl" }
                    };

                    foreach (var stomatolog in stomatolodzy)
                    {
                        var passwordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "HasloTest1");
                        stomatolog.PasswordHash = passwordHash;

                        var result = userManager.CreateAsync(stomatolog).Result;
                        if (result.Succeeded)
                        {
                            Console.WriteLine($"Utworzono użytkownika {stomatolog.UserName} pomyślnie!");
                            context.Stomatolodzy.Add(stomatolog);
                        }
                        else
                        {
                            Console.WriteLine($"Błąd podczas tworzenia użytkownika {stomatolog.UserName}: {result.Errors}");
                        }
                        context.SaveChanges();
                    }

                    if (!context.Wizyty.Any())
                    {
                        var random = new Random();
                        var stomatologIds = context.Stomatolodzy.Select(s => s.Id).ToList();

                        var wizyty = new UmowWizyteViewModel[]
                        {
                new UmowWizyteViewModel { WybranaData = DateTime.Now.AddDays(7), WybranaGodzina = "09:00", WybranyStomatologId = stomatologIds[random.Next(stomatologIds.Count)] },
                new UmowWizyteViewModel { WybranaData = DateTime.Now.AddDays(14), WybranaGodzina = "10:30", WybranyStomatologId = stomatologIds[random.Next(stomatologIds.Count)] },
                new UmowWizyteViewModel { WybranaData = DateTime.Now.AddDays(15), WybranaGodzina = "11:00", WybranyStomatologId = stomatologIds[random.Next(stomatologIds.Count)] },
                new UmowWizyteViewModel { WybranaData = DateTime.Now.AddDays(3), WybranaGodzina = "09:00", WybranyStomatologId = stomatologIds[random.Next(stomatologIds.Count)] },
                new UmowWizyteViewModel { WybranaData = DateTime.Now.AddDays(8), WybranaGodzina = "10:30", WybranyStomatologId = stomatologIds[random.Next(stomatologIds.Count)] },
                new UmowWizyteViewModel { WybranaData = DateTime.Now.AddDays(10), WybranaGodzina = "11:00", WybranyStomatologId = stomatologIds[random.Next(stomatologIds.Count)] },
                new UmowWizyteViewModel { WybranyStomatologId = "3", WybranaData = DateTime.Now.AddDays(7), WybranaGodzina = "09:00" },
                new UmowWizyteViewModel { WybranyStomatologId = "4", WybranaData = DateTime.Now.AddDays(14), WybranaGodzina = "10:00" },
                new UmowWizyteViewModel { WybranyStomatologId = "5", WybranaData = DateTime.Now.AddDays(15), WybranaGodzina = "10:30" },
                new UmowWizyteViewModel { WybranyStomatologId = "6", WybranaData = DateTime.Now.AddDays(3), WybranaGodzina = "11:00" },
                new UmowWizyteViewModel { WybranyStomatologId = "7", WybranaData = DateTime.Now.AddDays(8), WybranaGodzina = "09:00" }
                        };


                        context.Wizyty.AddRange(wizyty);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
        
    
    