namespace PaymentTestCase.Application.Interfaces;

public interface IBankGateway
{
    Task<(bool IsSuccess, string? BankReference)> PayAsync(string orderReference, decimal amount, CancellationToken ct);
    Task<(bool IsSuccess, string? BankReference)> CancelAsync(string orderReference, decimal amount, CancellationToken ct);
    Task<(bool IsSuccess, string? BankReference)> RefundAsync(string orderReference, decimal amount, CancellationToken ct);
}