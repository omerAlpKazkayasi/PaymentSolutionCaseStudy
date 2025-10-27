using PaymentTestCase.Domain.Common;

namespace PaymentTestCase.Domain.Entities;

public class TransactionDetail : BaseEntity
{
    public Guid TransactionId { get; set; }

    public string TransactionType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public Transaction? Transaction { get; set; }
}