using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Infrastructure.Banks;

public sealed class YapiKrediGateway : IBankGateway
{
    public Task<(bool, string?)> PayAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"YK-{orderReference}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> CancelAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"YK-CANCEL-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> RefundAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"YK-REFUND-{Guid.NewGuid():N}"));
}