using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Domain.Abstract;

public interface IBankRules
{
    bool CanCancel(Transaction transaction, DateTimeOffset now);
    bool CanRefund(Transaction transaction, DateTimeOffset now);
}