using Backend.Domain.Models;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Interfaces.User
{
    public interface IAccountRepository
    {
        public Task<Account> CreateAccount(Account account);
        public Task<Account> GetAccountByUserId(UserId userId);
    }
}
