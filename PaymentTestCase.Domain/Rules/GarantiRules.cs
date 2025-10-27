using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Domain.Rules;

public class GarantiRules : IBankRules
{
    public bool CanCancel(Transaction transaction, DateTimeOffset now)
        => transaction.TotalAmount <= 10000;

    public bool CanRefund(Transaction transaction, DateTimeOffset now)
        => transaction.TransactionDate.AddHours(12) < now;
}