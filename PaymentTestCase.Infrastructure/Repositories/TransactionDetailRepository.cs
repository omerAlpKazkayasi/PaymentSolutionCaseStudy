using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Entities;
using PaymentTestCase.Infrastructure.Persistence;

namespace PaymentTestCase.Infrastructure.Repositories;

public class TransactionDetailRepository : EfRepository<TransactionDetail>, ITransactionDetailRepository
{
    private readonly IPaymentDbContext _paymentDbContext;

    public TransactionDetailRepository(IPaymentDbContext paymentDbContext)
        : base((PaymentDbContext)paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

}