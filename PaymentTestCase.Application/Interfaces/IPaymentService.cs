using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Interfaces;

public interface IPaymentService
{
    Task PayAsync(string bank, Guid orderId, CancellationToken cancellationToken);

    Task CancelAsync(Guid orderId, decimal cancelAmount, CancellationToken cancellationToken);

    Task RefundAsync(Guid orderId, decimal amount, CancellationToken cancellationToken);
}