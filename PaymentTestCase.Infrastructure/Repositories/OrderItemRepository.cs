using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Entities;
using PaymentTestCase.Infrastructure.Persistence;

namespace PaymentTestCase.Infrastructure.Repositories;

public class OrderItemRepository : EfRepository<OrderItem>, IOrderItemRepository
{
    private readonly IPaymentDbContext _paymentDbContext;

    public OrderItemRepository(IPaymentDbContext paymentDbContext)
        : base((PaymentDbContext)paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

}