using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Infrastructure.Banks;

public sealed class GarantiGateway : IBankGateway
{
    public Task<(bool, string?)> PayAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"GA-{orderReference}-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> CancelAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"GA-CANCEL-{Guid.NewGuid():N}"));

    public Task<(bool, string?)> RefundAsync(string orderReference, decimal amount, CancellationToken ct)
        => Task.FromResult((true, $"GA-REFUND-{Guid.NewGuid():N}"));
}