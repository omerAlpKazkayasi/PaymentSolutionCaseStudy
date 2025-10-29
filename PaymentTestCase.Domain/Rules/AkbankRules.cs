using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Domain.Rules;

public class AkbankRules : IBankRules
{
    public bool CanCancel(Transaction transaction, DateTimeOffset now)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        return transaction.TransactionDate.Date == now.Date;
    }

    public bool CanRefund(Transaction transaction, DateTimeOffset now)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        return now.Date >= transaction.TransactionDate.Date.AddDays(1);
    }
}