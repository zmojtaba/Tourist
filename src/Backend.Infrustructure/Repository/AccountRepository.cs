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
            return account;
        }

        public async Task<Account?> GetAccountById(AccountId accId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accId);
        }

        public async Task<List<Account>> GetAllAccounts()
        {
            return await _context.Accounts.Include(a => a.Devices).ToListAsync();
        }
    }
}
