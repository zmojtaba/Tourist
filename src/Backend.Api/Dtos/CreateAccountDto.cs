namespace Backend.Api.Dtos
{
    public class CreateAccountDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string UserRole { get; set; } = string.Empty;

    }
}
