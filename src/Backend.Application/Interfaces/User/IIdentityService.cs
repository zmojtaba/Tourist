using Backend.Application.Features.Accounts;

namespace Backend.Application.Interfaces.User
{
    public interface IIdentityService
    {
        public Task<Guid> CreateUserAsync(string phoneNumber, string password, string role);
        public Task<bool> SendPhoneNumberVerificationCode(string phoneNumber);
        public Task<string> VerifyPhoneCodeAsync(string phoneNumber, int code);
        public Task<IdentityLogInResponse> LogInServiceAsync(string phoneNumber, string password);
    }
}
