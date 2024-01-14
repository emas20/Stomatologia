using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stomatologia.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stomatologia.Models;
using System;
using static Stomatologia.Data.ApplicationDbContext;
using Stomatologia.Interfaces;
using Stomatologia.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
//builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();
//obs³uga uwierzytelniania
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie();
builder.Services.AddAuthorization();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Konfiguracja kontekstu bazy danych dla Identity
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var identityBuilder = builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; 
});
identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
identityBuilder.AddDefaultTokenProviders();
identityBuilder.AddRoles<IdentityRole>();

builder.Services.AddScoped<IWizytyService, WizytyService>();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<StomatologService>();
var app = builder.Build();

// Inicjalizacja danych
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>(); // Pobranie UserManager dla Stomatolog
        string[] roleNames = { "User", "Stomatolog", "Admin" };
        IdentityResult roleResult;
        // Inicjalizacja roli
        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                // Tworzenie roli, jeœli nie istnieje
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        ApplicationDbContextSeed.Initialize(dbContext, userManager);

    }
    catch (Exception ex)
    {
        // Obs³uga b³êdu inicjalizacji danych
        Console.WriteLine("Wyst¹pi³ b³¹d podczas inicjalizacji bazy danych: " + ex.Message);
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature.Error;

            // Logowanie b³êdu
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError($"Wyst¹pi³ b³¹d: {exception}");

            context.Response.Redirect("/Home/Error");
        });
    });
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "user",
        pattern: "users/{action}/{id?}",
        defaults: new { controller = "User" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.MapRazorPages();

app.Run();