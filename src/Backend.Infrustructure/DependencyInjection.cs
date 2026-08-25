using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Backend.Infrustructure.Data;
using Backend.Infrustructure.Repository;
using Backend.Infrustructure.Services;
using Backend.Application.Interfaces;

namespace Backend.Infrustructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(
                "PostgresConnection") ?? "Server=localhost;Port=3306;Database=BookStoreDb;User=root;Password=password;";

            services.AddDbContext<ApplicationDbContext>(
                    options => options.UseNpgsql(connectionString
                    //o => o.UseNetTopologySuite()
                    ));


            // Identity configuration

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); ;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"])
                    )

                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var claims = context.Principal.Claims;
                        var tokenTypeClaim = claims.FirstOrDefault(c => c.Type == "token_type")?.Value;

                        // Check if the token type is "access_token"
                        if (tokenTypeClaim != "access_token")
                        {
                            context.Fail("Unauthorized"); // Reject the token if it's not an access token
                        }

                        return Task.CompletedTask;
                    }
                };


            });

            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IAgentRoleRepository, AgentRoleRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, TokenService>();


            return services;
        }
    }
}
