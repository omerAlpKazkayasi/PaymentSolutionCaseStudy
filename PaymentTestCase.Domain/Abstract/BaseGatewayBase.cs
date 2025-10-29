namespace PaymentTestCase.Domain.Abstract;

public abstract class BankGatewayBase
{
    public virtual Task<(bool IsSuccess, string? BankReference)> PayAsync(
        int orderNumber, decimal amount, CancellationToken cancellationToken)
    {
        return Task.FromResult((false, (string?)null));
    }

    public virtual Task<(bool IsSuccess, string? BankReference)> CancelAsync(
        int orderNumber, decimal amount, CancellationToken cancellationToken)
    {
        return Task.FromResult((false, (string?)null));
    }

    public virtual Task<(bool IsSuccess, string? BankReference)> RefundAsync(
        int orderNumber, decimal amount, CancellationToken cancellationToken)
    {
        return Task.FromResult((false, (string?)null));
    }
}