using Backend.Application.Common.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrustructure.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager =
                services.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                services.GetRequiredService<UserManager<ApplicationUser>>();

            var config =
                services.GetRequiredService<IConfiguration>();

            // Seed roles
            foreach (var role in RoleList.UserRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result =
                        await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            result.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(
                            $"Failed to create role '{role}': {errors}");
                    }
                }
            }

            // Seed admin
            var adminPhoneNumber =
                config["AdminUser:PhoneNumber"];

            var adminPassword =
                config["AdminUser:Password"];

            if (string.IsNullOrWhiteSpace(adminPhoneNumber))
                throw new InvalidOperationException(
                    "AdminUser:PhoneNumber is not configured.");

            if (string.IsNullOrWhiteSpace(adminPassword))
                throw new InvalidOperationException(
                    "AdminUser:Password is not configured.");

            var admin = await userManager.Users
                .FirstOrDefaultAsync(
                    u => u.PhoneNumber == adminPhoneNumber);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = Guid.NewGuid().ToString(),
                    PhoneNumber = adminPhoneNumber,
                    PhoneNumberConfirmed = true
                };

                var createResult =
                    await userManager.CreateAsync(admin, adminPassword);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to create admin user: {errors}");
                }
            }

            // Make sure admin has Admin role
            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var roleResult =
                    await userManager.AddToRoleAsync(admin, "Admin");

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to add admin role: {errors}");
                }
            }
        }
    }
}
