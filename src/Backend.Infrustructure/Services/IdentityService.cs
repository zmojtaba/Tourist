

using Backend.Application.Exceptions;
using Backend.Application.Features.Accounts;
using Backend.Application.Interfaces.User;
using Backend.Domain.ValueObjects;
using Backend.Infrustructure.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Backend.Infrustructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepo;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public IdentityService(IIdentityRepository identityRepo, IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _identityRepo = identityRepo;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async Task<string> CreateUserAsync(string phoneNumber, string password, string? email, string? role)
        {


            if (await _identityRepo.IsUserExisteByPhoneNumberAsync(phoneNumber))
                throw new BadRequestException("Phone number Already Exists.");
            string userId = await _identityRepo.CreateUserAsync(phoneNumber, password, email);
            userId = await _identityRepo.AddToRoleAsync(phoneNumber, role ?? "User");

            //bool canParse = Guid.TryParse(userId, out Guid userGuid);
            //if (!canParse) throw new InternalServerException("Some went wrong");
            return userId;
        }

        public async Task<bool> SendPhoneNumberVerificationCode(string phoneNumber)
        {
            if (!await _identityRepo.IsUserExisteByPhoneNumberAsync(phoneNumber))
                throw new BadRequestException("Phone number does not Exists.");
            int randomNumber = new Random().Next(100000, 999999);
            //using System.Security.Cryptography;

            int code = RandomNumberGenerator.GetInt32(100000, 1000000);
            await _identityRepo.SetPhoneVerificationCodeAsync(phoneNumber, code);

            ////////////////////////////// sending code with sms

            Console.WriteLine("===================================  " + code);
            return true;
        }

        public async Task<string> VerifyPhoneCodeAsync(string phoneNumber, int code)
        {
            bool result = await _identityRepo.VerifyPhoneCodeAsync(phoneNumber, code);
            if (result) return "Verified Successfully";
            return "Do Not Verify";
        }

        public async Task<IdentityLogInResponse> LogInServiceAsync(string phoneNumber, string password)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                throw new BadRequestException("PhoneNumber must be provided.");


            if (string.IsNullOrEmpty(password))
                throw new BadRequestException("Password must be provided.");


            bool isPasswordValid = await _identityRepo.CheckPasswordAsync(phoneNumber, password);

            if (!isPasswordValid)
            {
                throw new BadRequestException("Password is incorrect.");
            }

            ApplicationUserLogInHistory userHistory = new ApplicationUserLogInHistory
            {
                LoginTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                IpAddress = _httpContextAccessor.HttpContext?
                    .Connection.RemoteIpAddress?.ToString(),
                UserAgent = _httpContextAccessor.HttpContext?
                    .Request.Headers["User-Agent"].ToString(),
                IsSuccessful = isPasswordValid
            };

            //do it later
            //await _identityRepo.AddUserHistory(userHistory);


            string? role = await _identityRepo.GetUserRoleAsync(phoneNumber);

            if (role == null) throw new BadRequestException("This username has no role");
            string accessToken = _tokenService.CreateAccessToken(phoneNumber, role);
            string refreshToken = _tokenService.CreateRefreshToken(phoneNumber);
            string userId =  await _identityRepo.UpdateUserRefreshToken(phoneNumber, refreshToken);

            return new IdentityLogInResponse(UserId.Of(Guid.Parse(userId)), refreshToken, accessToken);
        }

    }
}
