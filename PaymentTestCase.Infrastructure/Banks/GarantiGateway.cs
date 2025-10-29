using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Infrastructure.Banks;

public sealed class GarantiGateway : IBankGateway
{
    public Task<(bool, string?)> PayAsync(int orderNumber, decimal amount, CancellationToken cancellationToken)
        => Task.FromResult((true, $"GA-{orderNumber}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> CancelAsync(int orderNumber, decimal amount, CancellationToken cancellationToken)
        => Task.FromResult((true, $"GA-CANCEL-{orderNumber}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> RefundAsync(int orderNumber, decimal amount, CancellationToken cancellationToken)
        => Task.FromResult((true, $"GA-REFUND-{orderNumber}-{Guid.NewGuid():N}"));
}