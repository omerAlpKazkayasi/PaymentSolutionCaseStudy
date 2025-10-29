using Microsoft.EntityFrameworkCore;
using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Entities;
using PaymentTestCase.Infrastructure.Persistence;

namespace PaymentTestCase.Infrastructure.Repositories;

public class OrderRepository : EfRepository<Order>, IOrderRepository
{
    private readonly IPaymentDbContext _paymentDbContext;

    public OrderRepository(IPaymentDbContext paymentDbContext)
        : base((PaymentDbContext)paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

    public async Task<Order> GetOrderWithOrderItems(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _paymentDbContext.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null || order.Items.Count == 0)
        {
            throw new KeyNotFoundException("Order or order items not found.");
        }
        else
        {
            return order;
        }
    }
}