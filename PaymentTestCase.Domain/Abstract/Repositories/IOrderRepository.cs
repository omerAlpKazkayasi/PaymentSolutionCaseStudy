using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Domain.Abstract.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetOrderWithOrderItems (Guid orderId, CancellationToken cancellationToken);
}