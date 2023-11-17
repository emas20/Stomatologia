using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stomatologia.Models;

namespace Stomatologia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       // protected override void OnModelCreating(ModelBuilder modelBuilder)
       // {
 
        
       // }
        public DbSet<Stomatologia.Models.User>? User { get; set; }
       // protected override void OnModelCreating(ModelBuilder modelBuilder)
       // {
 
        
       // }
        public DbSet<Stomatologia.Models.EditProfileViewModel>? EditProfileViewModel { get; set; }
    }

}