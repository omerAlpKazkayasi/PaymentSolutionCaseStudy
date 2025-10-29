using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Infrastructure.Banks;

public sealed class YapiKrediGateway : IBankGateway
{
    public Task<(bool, string?)> PayAsync(int orderNumber, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"YK-{orderNumber}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> CancelAsync(int orderNumber, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"YK-CANCEL-{orderNumber}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> RefundAsync(int orderNumber, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"YK-REFUND-{orderNumber}-{Guid.NewGuid():N}"));
}