


namespace Backend.Application.Features.Accounts
{
    public record VerifyPhoneNumberRequestCommand(string phoneNumber) : ICommand<bool>;
    public class VerifyPhoneNumberRequestHandler(IIdentityService identityService) : ICommandHandler<VerifyPhoneNumberRequestCommand, bool>
    {
        public async Task<bool> Handle(VerifyPhoneNumberRequestCommand command, CancellationToken cancellationToken)
        {
            bool result = await identityService.SendPhoneNumberVerificationCode(command.phoneNumber);
            return result;
        }
    }
}
