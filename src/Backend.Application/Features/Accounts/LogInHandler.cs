using Backend.Application.Interfaces.User;
using Backend.Domain.Models;
using Backend.Domain.ValueObjects;
using PhoneNumbers;

namespace Backend.Application.Features.Accounts
{
    public record LogInCommand(string PhoneNumber, string Password) : ICommand<LogInResponse>;
    public record LogInResponse(Guid AccountId, Guid UserId, string PhoneNumber, string RefreshToken, string AccessToken);
    public record IdentityLogInResponse(UserId Id, string RefreshToken, string AccessToken);

    public class LogInCommandValidator : AbstractValidator<LogInCommand>
    {
        public LogInCommandValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number cannot be empty")
                .Must(BeValidPhoneNumber)
                .WithMessage("Invalid phone number");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required");
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
    public class LogInHandler(IIdentityRepository userRepo,
            ITokenService tokenService,
            IIdentityService identityService,
            IAccountRepository accountRepo
            ) : ICommandHandler<LogInCommand, LogInResponse>
    {
        public async Task<LogInResponse> Handle(LogInCommand command, CancellationToken cancellationToken)
        {
            IdentityLogInResponse loginResponse = await identityService.LogInServiceAsync(command.PhoneNumber, command.Password);
            Account account = await accountRepo.GetAccountByUserId(loginResponse.Id);
            if (account == null)
            {
                account = Account.Create(AccountId.Of(Guid.NewGuid()), loginResponse.Id);
                account = await accountRepo.CreateAccount(account);
            }

            return new LogInResponse(
                account.Id.Value,
                loginResponse.Id.Value,
                command.PhoneNumber,
                loginResponse.RefreshToken,
                loginResponse.AccessToken
            );



        }
    }
}
