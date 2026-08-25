using Microsoft.AspNetCore.Identity;

namespace Backend.Infrustructure.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public string? PhoneVerificationCodeHash { get; set; }
        public DateTime? PhoneVerificationCodeExpiry { get; set; }
        public string? LogInCodeHash { get; set; }
        public DateTime? LogInCodeExpiry { get; set; }
    }
}
