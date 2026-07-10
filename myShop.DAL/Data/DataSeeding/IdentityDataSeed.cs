using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DAL.Data.DataSeeding
{
    public class IdentityDataSeed
    {

        public static async Task SeedIdentityDataAsync(UserManager<ApplicationUser> userManager ,
                RoleManager<IdentityRole> roleManager,
                ILogger logger) 
        {

            try
            {
                bool hasUsers = await userManager.Users.AnyAsync();
                bool hasRoles = await roleManager.Roles.AnyAsync();

                if (hasUsers && hasRoles)
                    return;

                // create roles
                // Create the following roles:
                //  Admin
                //  Customer
                var roles = new List<IdentityRole>
            {
                new ("Admin"),
                new ("Customer")

            };

                // add roles to the database
                foreach (var role in roles)
                {

                    // check if roles already exist in db
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {

                        var roleResult = await roleManager.CreateAsync(role); // returns IdentityResult
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Create Role {role.Name} : " +
                                $"{string.Join(";", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }
                ;

                if (!hasUsers)
                {

                    // create admin user
                    var adminUser = new ApplicationUser
                    {
                        Name = "Mohamed Hisham",
                        Email = "Mohamed@gmail.com",
                        UserName = "MohamedHisham",
                        PhoneNumber = "01021840100",
                        Address = "Egypt",
                        City = "Cairo"

                    };
                    await userManager.CreateAsync(adminUser, "P@ssw0rd");
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    var customer = new ApplicationUser
                    {

                        Name = "Ahmed Ali",
                        Email = "Ahmed@gmail.com",
                        UserName = "AhmedAli",
                        PhoneNumber = "01021840101",
                        Address = "Egypt",
                        City = "Monofia"
                    };
                    await userManager.CreateAsync(customer, "P@ssw0rd");
                    await userManager.AddToRoleAsync(customer, "Customer");

                    logger.LogInformation("Identity Data Seeded Successfully");

                }

                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity Seeding Failed");
                return;
            }
        }
    }
}
