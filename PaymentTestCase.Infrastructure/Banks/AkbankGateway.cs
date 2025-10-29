using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Infrastructure.Banks;

public sealed class AkbankGateway : IBankGateway
{
    public Task<(bool, string?)> PayAsync(int orderNumber, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"AK-{orderNumber}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> CancelAsync(int orderNumber, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"AK-CANCEL-{orderNumber}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> RefundAsync(int orderNumber, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"AK-REFUND-{orderNumber}-{Guid.NewGuid():N}"));
}