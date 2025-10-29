using Microsoft.EntityFrameworkCore;
using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Entities;
using PaymentTestCase.Infrastructure.Persistence;

namespace PaymentTestCase.Infrastructure.Repositories;

public class TransactionRepository : EfRepository<Transaction>, ITransactionRepository
{
    private readonly IPaymentDbContext _paymentDbContext;

    public TransactionRepository(IPaymentDbContext paymentDbContext)
        : base((PaymentDbContext)paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

    public async Task<Transaction?> GetByOrderAsync(string bank, string orderReference, CancellationToken ct)
    {
        //return await _paymentDbContext.Transactions
        //    .Include(t => t.Details)
        //    .FirstOrDefaultAsync(t => t.Bank == bank && t.OrderReference == orderReference, ct);

        return null;
    }    
}