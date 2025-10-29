namespace PaymentTestCase.Application.Interfaces;

public interface IBankGateway
{
    Task<(bool IsSuccess, string? BankReference)> PayAsync(int orderNumber, decimal amount, CancellationToken cancellationToken);
    Task<(bool IsSuccess, string? BankReference)> CancelAsync(int orderNumber, decimal amount, CancellationToken cancellationToken);
    Task<(bool IsSuccess, string? BankReference)> RefundAsync(int orderNumber, decimal amount, CancellationToken cancellationToken);
}