using Microsoft.AspNetCore.Identity;
using Stomatologia.Models;

namespace Stomatologia.Interfaces
{
    public interface IIdentityService
    {
        Task CreateUser(IdentityUser user, string password);
        Task AddUserToRole(IdentityUser user, string roleName);
        // Inne metody związane z zarządzaniem tożsamością
    }
}
