using System.Linq.Expressions;

namespace Backend.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public Task<T> ExecuteInTransactionAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct );
        public Task CompleteAsync();
    }
}
