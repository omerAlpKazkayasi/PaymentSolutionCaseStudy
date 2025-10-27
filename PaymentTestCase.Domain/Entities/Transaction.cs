using PaymentTestCase.Domain.Common;

namespace PaymentTestCase.Domain.Entities;

public class Transaction: BaseEntity
{
    public string Bank { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal NetAmount { get; set; }
    
    public string Status { get; set; } = string.Empty;

    public string OrderReference { get; set; } = string.Empty;

    public DateTimeOffset TransactionDate { get; set; }

    public ICollection<TransactionDetail> Details { get; set; } = new List<TransactionDetail>();
}