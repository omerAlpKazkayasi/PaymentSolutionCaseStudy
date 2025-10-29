using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Interfaces;

public interface IPaymentService
{
    Task PayAsync(string bank, Guid orderId, CancellationToken cancellationToken);

    Task CancelAsync(Guid orderId, Guid productId, int quantity, CancellationToken cancellationToken);

    Task RefundAsync(Guid orderId, Guid productId, int quantity, CancellationToken cancellationToken);
}