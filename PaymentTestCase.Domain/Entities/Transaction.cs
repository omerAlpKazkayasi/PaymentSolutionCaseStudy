using PaymentTestCase.Domain.Common;
using PaymentTestCase.Domain.Constants;

namespace PaymentTestCase.Domain.Entities;

public class Transaction : BaseEntity
{
    private List<TransactionDetail> _details = new();

    public string Bank { get; private set; } = default!;
    public Guid OrderId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal NetAmount { get; private set; }
    public DateTimeOffset TransactionDate { get; private set; }
    public string Status { get; private set; }
    public virtual ICollection<TransactionDetail> TransactionDetails
    {
        get => _details;
        private set => _details = value?.ToList() ?? new List<TransactionDetail>();
    }
    public Order Order { get; private set; }

    protected Transaction() { }

    public Transaction(Guid orderId, string bank, decimal totalAmount, string status, string type, decimal netAmount)
    {
        Id = Guid.NewGuid();
        SetOrderId(orderId);
        SetBank(bank);
        SetAmounts(totalAmount, type);
        TransactionDate = DateTimeOffset.UtcNow;
        SetStatus(status);
        SetNetAmount(netAmount, type);
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

    public void SetNetAmount(decimal amount, string type)
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

    public void SetAmounts(decimal total, string type)
    {
        if (!TransactionTypes.All.Contains(type))
            throw new ArgumentException($"Invalid transaction type: {type}", nameof(type));

        TotalAmount = type switch
        {
            TransactionTypes.Sale => total,
            TransactionTypes.Cancel => TotalAmount,
            TransactionTypes.Refund => TotalAmount,
            _ => TotalAmount
        };
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