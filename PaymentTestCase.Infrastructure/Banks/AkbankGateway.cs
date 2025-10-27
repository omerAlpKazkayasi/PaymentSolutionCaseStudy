using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Infrastructure.Banks;

public sealed class AkbankGateway : IBankGateway
{
    public Task<(bool, string?)> PayAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"AK-{orderReference}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> CancelAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"AK-CANCEL-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> RefundAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"AK-REFUND-{Guid.NewGuid():N}"));
}