

using Backend.Infrustructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddInfrastructureServices(builder.Configuration).AddApplicationServices().AddApiService(myAllowSpecificOrigins);
            builder.Services.AddMemoryCache();

            builder.WebHost.UseUrls("http://0.0.0.0:5001");



            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext =
                        services.GetRequiredService<ApplicationDbContext>();

                    // Apply pending EF Core migrations
                    await dbContext.Database.MigrateAsync();

                    // Seed Identity roles and admin user
                    await IdentitySeeder.SeedAsync(services);
                }
                catch (Exception ex)
                {
                    var logger =
                        services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(
                        ex,
                        "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            app.UseApiServices(myAllowSpecificOrigins);


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
