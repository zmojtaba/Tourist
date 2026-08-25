namespace Backend.Application.Features.Accounts
{
    public record CreateUserCommand(string PhoneNumber, string Password, string? Email, string UserRole) : ICommand<CreateUserResponse>;

    public record CreateUserResponse(Guid AccountId, Guid UserId, string PhoneNumber, string RefreshToken, string AccessToken);

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number cannot be empty")
                .Must(BeValidPhoneNumber)
                .WithMessage("Invalid phone number");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required");
            
            RuleFor(x => x.Email)
                .Must(BeValidEmail)
                .WithMessage("Invalid email format")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.UserRole)
                .NotEmpty().WithMessage("User Role is Required")
                .Must(BeValidUserRole)
                .WithMessage("Invalid User Role");
        }

        private bool BeValidUserRole(string? Role)
        {
            return RoleList.UserRoles.Contains(Role, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && email.Contains(".");
            }
            catch
            {
                return false;
            }
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

    public class CreateUserHandler(
        IIdentityService identityService, 
        ITokenService tokenService, 
        IIdentityRepository identityRepo) : ICommandHandler<CreateUserCommand, CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            string userId = await identityService.CreateUserAsync(command.PhoneNumber, command.Password, command.Email, command.UserRole);
            ////// user create in infru layer////
            ///should create one to one relation with user like Account////
            Account account = Account.Create(AccountId.Of(Guid.NewGuid()), UserId.Of(Guid.Parse(userId)));

            string refreshToken = tokenService.CreateRefreshToken(command.PhoneNumber);
            string accessToken = tokenService.CreateAccessToken(command.PhoneNumber, command.UserRole);
            await identityRepo.UpdateUserRefreshToken( command.PhoneNumber, refreshToken);

            return new CreateUserResponse(
                account.Id.Value, 
                Guid.Parse(userId), 
                command.PhoneNumber,
                refreshToken,
                accessToken
                );
        }
    }
}
