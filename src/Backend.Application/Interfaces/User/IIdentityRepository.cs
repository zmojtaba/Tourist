using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Interfaces.User
{
    public interface IIdentityRepository
    {
        public Task<string> CreateUserAsync(string phoneNum, string password, string? email);

        public Task<string?> GetUserIdByPhoneNumberAsync(string phoneNumber);
        public Task DeleteUserAsync(string phoneNumber);
        public Task<string> AddToRoleAsync(string phoneNum, string role);
        public Task<string?> GetUserRoleAsync(string phoneNumber);
        public Task<bool> IsUserExisteByPhoneNumberAsync(string phoneNumber);
        public Task SetPhoneVerificationCodeAsync(string phoneNumber, int code);
        public Task<bool> VerifyPhoneCodeAsync(string phoneNumber, int code);
        public Task<bool> CheckPasswordAsync(string phoneNumber, string password);
        public Task<IdentityResult> ResetPasswordAsync(string phoneNumber, string newPassword);
        public Task<string> UpdateUserRefreshToken(string phoneNumber, string refreshToken);

    }
}
