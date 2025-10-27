namespace PaymentTestCase.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public int BankId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string OrderReference { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }

    public ICollection<TransactionDetail> Details { get; set; } = new List<TransactionDetail>();
}