using Backend.Application.Common.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrustructure.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var config = services.GetRequiredService<IConfiguration>();

            // Roles
            List<string> roles = RoleList.UserRoles;

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Admin user
            var adminPhoneNumber = config["AdminUser:PhoneNumber"];
            var adminPassword = config["AdminUser:Password"];

            var admin = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(adminPhoneNumber));

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = Guid.NewGuid().ToString(),
                    PhoneNumber = adminPhoneNumber,
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(admin, adminPassword);
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
