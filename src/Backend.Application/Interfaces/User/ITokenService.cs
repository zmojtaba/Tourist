namespace Backend.Application.Interfaces.User
{
    public interface ITokenService
    {
        public string CreateAccessToken(AccountId id, string phoneNum, string role);
        public string CreateRefreshToken(AccountId id, string PhoneNum);
    }
}
