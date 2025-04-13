using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityService.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // 1. Define roles
            string[] roles = { "Manager", "StoreCustomer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Create test users
            var managerUser = new IdentityUser { UserName = "manager@test.com", Email = "manager@test.com", EmailConfirmed = true };
            var customerUser = new IdentityUser { UserName = "customer@test.com", Email = "customer@test.com", EmailConfirmed = true };

            await CreateUserIfNotExists(userManager, managerUser, "Manager", "Manager123$", new[] { "read", "create", "update", "delete" });
            await CreateUserIfNotExists(userManager, customerUser, "StoreCustomer", "Customer123$", new[] { "read" });
        }

        private static async Task CreateUserIfNotExists(UserManager<IdentityUser> userManager, IdentityUser user, string role, string password, string[] permissions)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
                return;

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                return;

            await userManager.AddToRoleAsync(user, role);

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));

            foreach (var permission in permissions)
            {
                await userManager.AddClaimAsync(user, new Claim("permission", permission));
            }
        }
    }
}
