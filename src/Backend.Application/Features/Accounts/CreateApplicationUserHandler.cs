using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Features.Accounts
{
    public record CreateApplicationUserCommand(string PhoneNumber, string Password, string ConfirmPassword, string UserRole) : ICommand<CreateUserResponse>;

    public record CreateUserResponse(Guid AccountId, string PhoneNumber, string RefreshToken, string AccessToken);

    public class CreateUserCommandValidator : AbstractValidator<CreateApplicationUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number cannot be empty")
                .Must(BeValidPhoneNumber)
                .WithMessage("Invalid phone number");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required");;

            RuleFor(x => x)
                .Must(x =>
                {
                    return x.Password.Equals(x.ConfirmPassword);
                })
                .WithMessage("Password and Confirm Passwod must be the same.");

            RuleFor(x => x.UserRole)
                .NotEmpty().WithMessage("User Role is Required")
                .Must(BeValidUserRole)
                .WithMessage("Invalid User Role");
        }

        private bool BeValidUserRole(string? Role)
        {
            return RoleList.UserRoles.Contains(Role, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            var phoneUtil = PhoneNumberUtil.GetInstance();

            try
            {
                if (!phone.StartsWith("+")) return false;
                // "NL" = default region (important if number has no +)
                var number = phoneUtil.Parse(phone, null);

                return phoneUtil.IsValidNumber(number);
            }
            catch
            {
                return false;
            }
        }
    }

    public class CreateApplicationUserHandler(
        IIdentityService identityService, 
        ITokenService tokenService, 
        IIdentityRepository identityRepo,
        IUnitOfWork uow,
        IAccountRepository accountRepo,
        IHttpContextAccessor httpContextAccessor) : ICommandHandler<CreateApplicationUserCommand, CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(CreateApplicationUserCommand command, CancellationToken ct)
        {

            var httpContext = httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context unavailable");

            // IP
            var ip = httpContext.Connection.RemoteIpAddress?.ToString();

            // Browser/device information
            var userAgent = httpContext.Request.Headers.UserAgent.ToString();

            CreateUserResponse userResponse =  await uow.ExecuteInTransactionAsync(async (ct) => 
            {
                Guid userId = await identityService.CreateUserAsync(command.PhoneNumber, command.Password, command.UserRole);
                Account account = Account.Create(AccountId.Of(userId));

                Device device = Device.Create(account.Id, "name will considet later", userAgent, ip, "location is get later");

                account.AddDevice(device);

                await accountRepo.CreateAccount(account);

                string refreshToken = tokenService.CreateRefreshToken(account.Id, command.PhoneNumber);
                string accessToken = tokenService.CreateAccessToken(account.Id, command.PhoneNumber, command.UserRole);
                await identityRepo.UpdateUserRefreshToken(command.PhoneNumber, refreshToken);
                return new CreateUserResponse(
                    account.Id.Value,
                    command.PhoneNumber,
                    refreshToken,
                    accessToken
                );

            }, ct);

            return userResponse;
        }
    }
}
