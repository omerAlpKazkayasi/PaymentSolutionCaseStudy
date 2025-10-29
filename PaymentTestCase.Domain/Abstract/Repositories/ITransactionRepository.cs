using PaymentTestCase.Domain.Entities;
using System.Linq.Expressions;

namespace PaymentTestCase.Domain.Abstract.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<Transaction> GetTransactionWithTransactionDetails(Guid orderId, CancellationToken cancellationToken);

    Task<IEnumerable<Transaction>> GetWithDetailsAsync(Expression<Func<Transaction, bool>>? predicate, CancellationToken cancellationToken);
}