using Microsoft.EntityFrameworkCore;
using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Constants;
using PaymentTestCase.Domain.Entities;
using PaymentTestCase.Infrastructure.Persistence;
using System.Linq.Expressions;
using System.Linq;

namespace PaymentTestCase.Infrastructure.Repositories;

public class TransactionRepository : EfRepository<Transaction>, ITransactionRepository
{
    private readonly IPaymentDbContext _paymentDbContext;

    public TransactionRepository(IPaymentDbContext paymentDbContext)
        : base((PaymentDbContext)paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

    public async Task<Transaction> GetTransactionWithTransactionDetails(Guid orderId, CancellationToken cancellationToken)
    {
        var transaction = await _paymentDbContext.Transactions
            .Include(o => o.TransactionDetails)
            .FirstOrDefaultAsync(o =>
                o.OrderId == orderId &&
                o.Status == TransactionStatuses.Success,
                cancellationToken);

        if (transaction is null || transaction.TransactionDetails.Count == 0)
        {
            throw new KeyNotFoundException("transaction or transaction details not found.");
        }
        else
        {
            return transaction;
        }
    }

    public async Task<IEnumerable<Transaction>> GetWithDetailsAsync(
        Expression<Func<Transaction, bool>>? predicate,
        CancellationToken cancellationToken)
    {
        var query = _paymentDbContext.Transactions
            .Include(x => x.TransactionDetails)
            .AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync(cancellationToken);
    }
}