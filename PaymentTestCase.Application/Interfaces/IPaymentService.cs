using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Interfaces;

public interface IPaymentService
{
    Task<Transaction> PayAsync(string bankId, string orderReference, decimal amount, CancellationToken ct);
    Task<Transaction> CancelAsync(string bankId, string orderReference, decimal amount, CancellationToken ct);
    Task<Transaction> RefundAsync(string bankId, string orderReference, decimal amount, CancellationToken ct);
}