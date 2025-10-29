using PaymentTestCase.Domain.Common;
using PaymentTestCase.Domain.Constants;

namespace PaymentTestCase.Domain.Entities;

public class Transaction : BaseEntity
{
    private readonly List<TransactionDetail> _details = new();

    public string Bank { get; private set; } = default!;
    public Guid OrderId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal NetAmount { get; private set; }
    public DateTimeOffset TransactionDate { get; private set; }
    public string Status { get; private set; }
    public IReadOnlyCollection<TransactionDetail> TransactionDetails => _details.AsReadOnly();
    public Order Order { get; private set; }

    protected Transaction() { }

    public Transaction(Guid orderId, string bank, decimal totalAmount, string status)
    {
        Id = Guid.NewGuid();
        SetOrderId(orderId);
        SetBank(bank);
        SetAmounts(totalAmount);
        TransactionDate = DateTimeOffset.UtcNow;
        SetStatus(status);
    }

    public void SetOrderId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("OrderId cannot be empty.", nameof(OrderId));
        OrderId = value;
    }

    public void SetBank(string value)
    {
        if (!BankCodes.All.Contains(value))
            throw new ArgumentNullException(nameof(Bank), "Bank name cannot be empty.");
        Bank = value;
    }

    public void SetNetAmount(decimal amount ,string type)
    {
        if (!TransactionTypes.All.Contains(type))
            throw new ArgumentException($"Invalid transaction type: {type}", nameof(type));

        switch (type)
        {
            case TransactionTypes.Sale:
                NetAmount = amount;
                break;
            case TransactionTypes.Cancel:
                NetAmount -= amount;
                break;
            case TransactionTypes.Refund:
                NetAmount -= amount;
                break;
            default:
                break;
        }
    }

    public void SetAmounts(decimal total)
    {
        if (total < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(total));
        TotalAmount = total;
    }

    public void SetStatus(string value)
    {
        if (!TransactionStatuses.All.Contains(value))
            throw new ArgumentException($"Invalid transaction status: {value}", nameof(Status));
        Status = value;
    }

    public void AddDetail(string type, decimal amount, string status)
    {
        if (!TransactionStatuses.All.Contains(status))
            throw new ArgumentException($"Invalid detail status: {status}", nameof(status));
        var detail = new TransactionDetail(Id, type, status, amount);
        _details.Add(detail);
    }
}