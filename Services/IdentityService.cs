using Microsoft.AspNetCore.Identity;
using Stomatologia.Models;
using Stomatologia.Interfaces;

namespace Stomatologia.Services
{
    public class IdentityService : IIdentityService
        
{
    private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateUser(IdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Użytkownik został pomyślnie utworzony
            }
            else
            {
                // Obsługa błędów podczas tworzenia użytkownika
                foreach (var error in result.Errors)
                {
                    // Obsługa błędów, np. logowanie, zapis do dziennika zdarzeń, zwracanie informacji o błędzie
                }
            }
        }
        public async Task AddUserToRole(IdentityUser user, string roleName)
        {
            var userInDb = await _userManager.FindByIdAsync(user.Id);
            var role = await _userManager.FindByNameAsync(roleName);

            //if (userInDb != null && role != null)
           // {
            //    var result = await _userManager.AddToRoleAsync(userInDb, role);

            //    if (result.Succeeded)
             //   {
                    // Użytkownik został pomyślnie dodany do roli
              //  }
              //  else
              //  {
                    // Obsługa błędów podczas dodawania użytkownika do roli
               //     foreach (var error in result.Errors)
               //     {
                        // Obsługa błędów, np. logowanie, zapis do dziennika zdarzeń, zwracanie informacji o błędzie
                //    }
             //   }
          //  }
          //  else
           // {
                // Użytkownik lub rola nie istnieje
           // }
        }

    // Implementacje innych metod z interfejsu IIdentityService
}
}

