using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Entities;
using PaymentTestCase.Infrastructure.Persistence;

namespace PaymentTestCase.Infrastructure.Repositories;

public class ProductRepository : EfRepository<Product>, IProductRepository
{
    private readonly IPaymentDbContext _paymentDbContext;

    public ProductRepository(IPaymentDbContext paymentDbContext)
        : base((PaymentDbContext)paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

}