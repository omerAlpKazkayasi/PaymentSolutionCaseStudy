using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Domain.Rules;

public class YapiKrediRules : IBankRules
{
    public bool CanCancel(Transaction transaction, DateTimeOffset now)
        => transaction.Status == "Success";

    public bool CanRefund(Transaction transaction, DateTimeOffset now)
        => now >= transaction.TransactionDate.AddDays(2);
}