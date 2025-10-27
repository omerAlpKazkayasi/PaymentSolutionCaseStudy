using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Constants;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly ITransactionRepository _transactions;
    private readonly IBankGatewayFactory _gatewayFactory;
    private readonly IBankRulesFactory _rulesFactory;

    public PaymentService(
        ITransactionRepository transactions,
        IBankGatewayFactory gatewayFactory,
        IBankRulesFactory rulesFactory)
    {
        _transactions = transactions;
        _gatewayFactory = gatewayFactory;
        _rulesFactory = rulesFactory;
    }

    public async Task<Transaction> PayAsync(string bank, string orderReference, decimal paymentAmount, CancellationToken cancellationToken)
    {
        var bankGateway = _gatewayFactory.Resolve(bank);
        var (isPaymentSuccessful, bankTransactionReference) = await bankGateway.PayAsync(orderReference, paymentAmount, cancellationToken);

        if (!isPaymentSuccessful)
        {
            return null;
        }
        var newTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Bank = bank,
            TotalAmount = paymentAmount,
            NetAmount = isPaymentSuccessful ? paymentAmount : 0,
            Status = isPaymentSuccessful ? TransactionStatuses.Success : TransactionStatuses.Fail,
            OrderReference = orderReference,
            TransactionDate = DateTimeOffset.UtcNow
        };

        newTransaction.Details.Add(new TransactionDetail
        {
            Id = Guid.NewGuid(),
            TransactionType = TransactionTypes.Sale,
            Status = newTransaction.Status,
            Amount = paymentAmount
        });

        await _transactions.AddAsync(newTransaction, cancellationToken);
        await _transactions.SaveChangesAsync(cancellationToken);

        return newTransaction;
    }

    public async Task<Transaction> CancelAsync(string bank, string orderReference, decimal cancelAmount, CancellationToken cancellationToken)
    {
        var existingTransaction = await _transactions.GetByOrderAsync(bank, orderReference, cancellationToken)
            ?? throw new KeyNotFoundException("Transaction not found.");

        var bankRules = _rulesFactory.Resolve(bank);
        if (!bankRules.CanCancel(existingTransaction, DateTimeOffset.UtcNow))
            throw new InvalidOperationException("Cancellation is not allowed for this transaction.");

        var bankGateway = _gatewayFactory.Resolve(bank);
        var (isCancelSuccessful, bankReference) = await bankGateway.CancelAsync(orderReference, cancelAmount, cancellationToken);
        if (!isCancelSuccessful) return existingTransaction;

        existingTransaction.NetAmount = Math.Max(0, existingTransaction.NetAmount - cancelAmount);
        existingTransaction.Details.Add(new TransactionDetail
        {
            Id = Guid.NewGuid(),
            TransactionType = TransactionTypes.Cancel,
            Status = TransactionStatuses.Success,
            Amount = cancelAmount
        });

        await _transactions.SaveChangesAsync(cancellationToken);
        return existingTransaction;
    }

    public async Task<Transaction> RefundAsync(string bankId, string orderReference, decimal refundAmount, CancellationToken cancellationToken)
    {
        var existingTransaction = await _transactions.GetByOrderAsync(bankId, orderReference, cancellationToken)
            ?? throw new KeyNotFoundException("Transaction not found.");

        var bankRules = _rulesFactory.Resolve(bankId);
        if (!bankRules.CanRefund(existingTransaction, DateTimeOffset.UtcNow))
            throw new InvalidOperationException("Refund is not allowed for this transaction.");

        var bankGateway = _gatewayFactory.Resolve(bankId);
        var (isRefundSuccessful, bankReference) = await bankGateway.RefundAsync(orderReference, refundAmount, cancellationToken);
        if (!isRefundSuccessful) return existingTransaction;

        existingTransaction.NetAmount = Math.Max(0, existingTransaction.NetAmount - refundAmount);
        existingTransaction.Details.Add(new TransactionDetail
        {
            Id = Guid.NewGuid(),
            TransactionType = TransactionTypes.Refund,
            Status = TransactionStatuses.Success,
            Amount = refundAmount
        });

        await _transactions.SaveChangesAsync(cancellationToken);
        return existingTransaction;
    }
}