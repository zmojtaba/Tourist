namespace Backend.Application.Features.Accounts
{
    public record GetAccountByIdQuery(Guid Id) : IQuery<Account>;
    public class GetAccountByIdHandler(IAccountRepository accRepo) : IQueryHandler<GetAccountByIdQuery, Account>
    {
        public async Task<Account> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            Account acc = await accRepo.GetAccountById(AccountId.Of(request.Id));
            return acc;
        }
    }
}
