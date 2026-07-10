using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.DataAccess;
using myshop.Entities.Models;
using myShop.DAL.Data.DataSeeding;

namespace myshop.PL
{
    public static class ProgramExtension
    {
        // data seed => extension method  return webapplication (app)

        public static async Task SeedIdentityDataAsync(this WebApplication app) {

            try
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync();
                }

                await IdentityDataSeed.SeedIdentityDataAsync(userManager, roleManager, app.Logger);
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "An error occurred while applying migrations or seeding identity data.");
                throw;
            }

        }
         
    }
}
