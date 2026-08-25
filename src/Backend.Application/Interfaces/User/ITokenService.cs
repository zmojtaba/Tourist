namespace Backend.Application.Interfaces.User
{
    public interface ITokenService
    {
        public string CreateAccessToken(string phoneNum, string role);
        public string CreateRefreshToken(string PhoneNum);
    }
}
