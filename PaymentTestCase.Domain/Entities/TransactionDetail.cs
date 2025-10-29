using PaymentTestCase.Domain.Common;
using PaymentTestCase.Domain.Constants;

namespace PaymentTestCase.Domain.Entities;

public class TransactionDetail : BaseEntity
{
    public Guid TransactionId { get; private set; }
    public string TransactionType { get; private set; }
    public string Status { get; private set; }
    public decimal Amount { get; private set; }

    protected TransactionDetail() { }

    public TransactionDetail(Guid transactionId, string type, string status, decimal amount)
    {
        Id = Guid.NewGuid();
        SetTransactionId(transactionId);
        SetTransactionType(type);
        SetStatus(status);
        SetAmount(amount);
    }

    public void SetTransactionId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("TransactionId cannot be empty.", nameof(TransactionId));
        TransactionId = value;
    }

    public void SetTransactionType(string value)
    {
        if (!TransactionTypes.All.Contains(value))
            throw new ArgumentNullException(nameof(TransactionType));
        TransactionType = value;
    }

    public void SetStatus(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(Status));
        Status = value;
    }

    public void SetAmount(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(Amount));
        Amount = value;
    }
}