namespace Backend.Application.Features.Accounts
{
    //public record GetAccountsResponse(Account Account);
    public record GetAccountsQuery() : IQuery<List<Account>>;
    public class GetAccountsHandler(IAccountRepository accountRepo) : IQueryHandler<GetAccountsQuery, List<Account>>
    {
        public async Task<List<Account>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var reslult = await accountRepo.GetAllAccounts();
            return reslult;
        }
    }
}
