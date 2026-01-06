using CoursesOnline.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesOnline.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await CreateRoleAsync(roleManager, "Admin");
            await CreateRoleAsync(roleManager, "User");

            // Admin
            var adminEmail = "admin@prueba.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Sistema",
                    LastName = "Administrador",
                    EmailConfirmed = true
                };

                    if ((await userManager.CreateAsync(admin, "Admin123!")).Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }

            // User
            var userEmail = "usuario@prueba.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new User
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "Pepito",
                    LastName = "Pérez",
                    EmailConfirmed = true
                };

                if ((await userManager.CreateAsync(user, "User123!")).Succeeded)
                    await userManager.AddToRoleAsync(user, "User");
            }
        }

        private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
