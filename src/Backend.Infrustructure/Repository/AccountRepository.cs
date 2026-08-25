using Backend.Application.Interfaces.User;
using Backend.Domain.ValueObjects;
using Backend.Infrustructure.Data;

namespace Backend.Infrustructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAccount(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> GetAccountByUserId(UserId userId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
        }
    }
}
