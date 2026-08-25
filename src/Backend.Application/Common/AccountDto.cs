namespace Backend.Application.Common
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
    }
}
