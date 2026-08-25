
namespace Backend.Application.Features.Accounts
{
    public record VerifyPhoneNumberCommand(string PhoneNumber, int Code) : ICommand<string>;
    public class VerifyPhoneNumberHandler(IIdentityService identityService) : ICommandHandler<VerifyPhoneNumberCommand, string>
    {
        public async Task<string> Handle(VerifyPhoneNumberCommand command, CancellationToken cancellationToken)
        {
            string result = await identityService.VerifyPhoneCodeAsync(command.PhoneNumber, command.Code);
            return result;
        }
    }
}
