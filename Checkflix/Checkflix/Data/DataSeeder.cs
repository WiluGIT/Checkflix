using Checkflix.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Data
{
    public class DataSeeder
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole adminRole = new IdentityRole();
                adminRole.Name = "Admin";
                IdentityResult result = roleManager.CreateAsync(adminRole).Result;
            }


            if (!roleManager.RoleExistsAsync("Teacher").Result)
            {
                IdentityRole userRole = new IdentityRole();
                userRole.Name = "User";
                IdentityResult result = roleManager.CreateAsync(userRole).Result;
            }
        }

        public static void SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@admin.pl").Result == null)
            {
                ApplicationUser adminUser = new ApplicationUser();
                adminUser.UserName = "admin@admin.pl";
                adminUser.Email = "admin@admin.pl";
                adminUser.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(adminUser, "Admin!23.").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
    }
}
