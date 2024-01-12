using Stomatologia.Models;
using Stomatologia.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Stomatologia.Services

{
    public class ApplicationUserService : IUserService
    {
    private readonly UserManager<IdentityUser> _userManager;

        public ApplicationUserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }


    }
}
