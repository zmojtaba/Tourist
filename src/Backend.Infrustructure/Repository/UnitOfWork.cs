using Backend.Application.Interfaces;
using Backend.Infrustructure.Data;

namespace Backend.Infrustructure.Repository
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(ct);
            try
            {
                var result = await action(ct);
                await context.SaveChangesAsync();
                await transaction.CommitAsync(ct);
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                throw new InfrustructureException(ex.ToString());
            }
        }
    }
}
