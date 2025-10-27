using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Domain.Abstract;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<Transaction?> GetByOrderAsync(string bankId, string orderReference, CancellationToken ct);
}