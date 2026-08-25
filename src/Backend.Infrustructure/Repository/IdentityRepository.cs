using System.Globalization;

namespace Backend.Infrustructure.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public IdentityRepository(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }


        public async Task<string> CreateUserAsync(string phoneNum, string password, string? email)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = Guid.NewGuid().ToString(),
                PhoneNumber = phoneNum,
                Email = email
            };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

            return user.Id;
        }

        public async Task<string?> GetUserIdByPhoneNumberAsync(string phoneNumber)
        {
            ApplicationUser? result = await GetUserByPhoneNumberAsync(phoneNumber);
            return result.Id;
        }
        private async Task<ApplicationUser> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            ApplicationUser? result = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(phoneNumber));

            return result ?? throw new NotFoundException("User Not Found");
        }

        public async Task<bool> IsUserExisteByPhoneNumberAsync(string phoneNumber)
        {
            return await _userManager.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task DeleteUserAsync(string phoneNumber)
        {
            ApplicationUser result = await GetUserByPhoneNumberAsync(phoneNumber);
            await _userManager.DeleteAsync(result);
        }


        public async Task<string> AddToRoleAsync(string phoneNum, string role)
        {
            ApplicationUser user = await GetUserByPhoneNumberAsync(phoneNum);
            role = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(role);
            var resutl =  await _userManager.AddToRoleAsync(user, role);
            if (!resutl.Succeeded)
            {

                ////////////////////////// focuse on user model ///////////////////////
                ///if role does not add to user then should remove user or not? /////////
                await _userManager.DeleteAsync(user);
                throw new InfrustructureException(string.Join(", ", resutl.Errors.Select(e => e.Description)));
            }

            return user.Id;
        }




        public async Task<string?> GetUserRoleAsync(string phoneNumber)
        {
            ApplicationUser user = await GetUserByPhoneNumberAsync(phoneNumber);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        public async Task<string> UpdateUserRefreshToken(string phoneNumber, string refreshToken)
        {
            ApplicationUser user = await GetUserByPhoneNumberAsync(phoneNumber);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            return user.Id;
        }

        public async Task<bool> CheckPasswordAsync(string phoneNumber, string password)
        {
            ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Equals(phoneNumber));
            if (user == null) throw new NotFoundException("User Not Found.");
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string phoneNumber, string newPassword)
        {
            ApplicationUser user = await GetUserByPhoneNumberAsync(phoneNumber);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }


        public async Task SetPhoneVerificationCodeAsync(string phoneNumber, int code)
        {
            ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null) throw new NotFoundException("User with this Phone Number not found");
            if (user.PhoneVerificationCodeExpiry != null &&
                user.PhoneVerificationCodeExpiry > DateTime.UtcNow)
            {
                throw new BadRequestException("Code already sent. Try again later.");
            }

            // here change to hash the phone code
            string hashedCode = _passwordHasher.HashPassword(user, code.ToString());

            user.PhoneVerificationCodeHash = hashedCode;
            user.PhoneVerificationCodeExpiry = DateTime.UtcNow.AddMinutes(5);

            await _userManager.UpdateAsync(user);
        }
        public async Task<bool> VerifyPhoneCodeAsync(string phoneNumber, int code)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null)
                throw new NotFoundException("User not found");

            if (user.PhoneVerificationCodeExpiry < DateTime.UtcNow)
                throw new Exception("Code expired");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PhoneVerificationCodeHash,
                code.ToString()
            );

            if (result == PasswordVerificationResult.Success) user.PhoneNumberConfirmed = true;

            return result == PasswordVerificationResult.Success;
        }


        //public async Task<string> GetUserIdByPhoneNumberAsync(string phoneNumber)
        //{

        //}

    }
}
