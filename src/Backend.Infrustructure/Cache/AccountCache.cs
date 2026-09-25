using Backend.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Backend.Infrustructure.Cache
{
    public class AccountCache : IAccountRepository
    {
        private readonly IAccountRepository _inerRepo;
        private readonly IDistributedCache _cache;
        private readonly ILogger<AccountCache> _logger;
        public AccountCache(IAccountRepository inerRepo, IDistributedCache cache, ILogger<AccountCache> logger)
        {
            _inerRepo = inerRepo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Account> CreateAccount(Account account)
        {
            await _inerRepo.CreateAccount(account);
            try
            {
                var key = $"account:{account.Id.Value}";
                var cacheOptions =  new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),            
                };
                await _cache.SetStringAsync(key, JsonSerializer.Serialize<Account>(account), cacheOptions);
                Console.WriteLine("**Put on memory");
            }catch (Exception ex)
            {
                _logger.LogError($"\n \n Redis Issue: {ex.ToString()} \n \n" );
            }
            return account;
        }

        public async Task<Account> GetAccountById(AccountId accId)
        {
            var key = $"account:{accId.Value}";
            Console.WriteLine("\n \n in cache account");
            try
            {
                var json = await _cache.GetStringAsync(key);
                if (json is not null)
                    return JsonSerializer.Deserialize<Account>(json);
            }catch (Exception ex)
            {
                _logger.LogError($"\n \n Redis Issue: {ex.ToString()} \n \n");
            }



            Account account =  await _inerRepo.GetAccountById(accId);
            if (account == null)
                return null;
            try
            {
                await _cache.SetStringAsync(key, JsonSerializer.Serialize<Account>(account), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
                Console.WriteLine("get from database");

            }catch (Exception ex)
            {
                _logger.LogError($"\n \n Redis Issue: {ex.ToString()} \n \n");
            }

            return account;

        }

        public async Task<List<Account>> GetAllAccounts()
        {
            List<Account> accounts = await _inerRepo.GetAllAccounts();
            return accounts;
        }
    }
}
