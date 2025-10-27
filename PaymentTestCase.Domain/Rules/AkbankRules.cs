using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Domain.Rules;

public class AkbankRules : IBankRules
{
    public bool CanCancel(Transaction transaction, DateTimeOffset now)
        => transaction.TransactionDate.Date == now.Date;

    public bool CanRefund(Transaction transaction, DateTimeOffset now)
        => now >= transaction.TransactionDate.AddDays(1);
}