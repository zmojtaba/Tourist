namespace Backend.Application.Common
{
    public class AppUserDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? EmailConfirmed { get; set; }
        public string? RefreshToken { get; set; }
        public string? PhoneNumber { get; set; }


    }
}
