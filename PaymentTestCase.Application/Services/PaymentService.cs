using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Constants;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankGatewayFactory _gatewayFactory;
    private readonly IBankRulesFactory _rulesFactory;
    private readonly IOrderRepository _orderRepository;
    private readonly IStockRepository _stockRepository;

    public PaymentService(
        ITransactionRepository transactionRepository,
        IBankGatewayFactory gatewayFactory,
        IBankRulesFactory rulesFactory,
        IOrderRepository orderRepository,
        IStockRepository stockRepository)
    {
        _transactionRepository = transactionRepository;
        _gatewayFactory = gatewayFactory;
        _rulesFactory = rulesFactory;
        _orderRepository = orderRepository;
        _stockRepository = stockRepository;
    }

    public async Task PayAsync(string bank, Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithOrderItems(orderId, cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException("Order not found.");
        }

        foreach (var item in order.Items)
        {
            var stockItem = await _stockRepository.GetAsync(s => s.ProductId == item.ProductId, cancellationToken);

            if (stockItem is null || stockItem.Count == 0)
            {
                throw new InvalidOperationException($"There is no stock data for {item.ProductId}");
            }
            else
            {
                if (stockItem[0].Quantity < item.Quantity)
                {
                    throw new InvalidOperationException($"there are no {item.Quantity} stock for product {item.ProductId}");
                }
                else
                {
                    stockItem[0].Decrease(item.Quantity);

                    await _stockRepository.UpdateAsync(stockItem[0].Id, stockItem[0], cancellationToken);
                }
            }
        }

        var bankGateway = _gatewayFactory.Resolve(bank);

        var (isPaymentSuccessful, bankTransactionReference) = await bankGateway.PayAsync(order.OrderNumber, order.TotalAmount, cancellationToken);

        var status = isPaymentSuccessful ? TransactionStatuses.Success : TransactionStatuses.Fail;

        var newTransaction = new Transaction(
            orderId: orderId,
            bank: bank,
            totalAmount: order.TotalAmount,
            status: status);

        newTransaction.AddDetail(TransactionTypes.Sale, newTransaction.TotalAmount, status);

        await _transactionRepository.AddAsync(newTransaction, cancellationToken);
        await _transactionRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelAsync(Guid orderId, decimal cancelAmount, CancellationToken cancellationToken)
    {
        var existingTransaction = await _transactionRepository.GetAsync(
            t =>
                t.OrderId == orderId &&
                t.Status == TransactionStatuses.Success,
                cancellationToken);

        if (existingTransaction == null || existingTransaction.Count == 0)
        {
            throw new KeyNotFoundException("Transaction not found.");
        }

        var transaction = existingTransaction[0];
        var bank = transaction.Bank;

        var bankRules = _rulesFactory.Resolve(bank);

        if (!bankRules.CanCancel(transaction, DateTimeOffset.UtcNow))
        {
            throw new InvalidOperationException("Cancellation is not allowed for this transaction.");
        }

        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException("Order not exist");
        }

        if (cancelAmount > transaction.NetAmount)
        {
            throw new InvalidOperationException("Cancel amount exceeds the net amount of the transaction.");
        }

        var bankGateway = _gatewayFactory.Resolve(bank);

        var (isCancelSuccessful, bankReference) = await bankGateway.CancelAsync(order.OrderNumber, cancelAmount, cancellationToken);

        var status = isCancelSuccessful ? TransactionStatuses.Success : TransactionStatuses.Fail;

        //stok geri eklemem gerekebilir eğer successe

        transaction.SetNetAmount(cancelAmount, TransactionTypes.Cancel);
        transaction.AddDetail(status: status, amount: cancelAmount, type: TransactionTypes.Cancel);

        await _transactionRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RefundAsync(Guid orderId, decimal refundAmount, CancellationToken cancellationToken)
    {
        var existingTransaction = await _transactionRepository.GetAsync(
            t =>
                t.OrderId == orderId &&
                t.Status == TransactionStatuses.Success,
                cancellationToken);

        if (existingTransaction == null || existingTransaction.Count == 0)
        {
            throw new KeyNotFoundException("Transaction not found.");
        }

        var transaction = existingTransaction[0];
        var bank = transaction.Bank;

        var bankRules = _rulesFactory.Resolve(bank);

        if (!bankRules.CanRefund(transaction, DateTimeOffset.UtcNow))
        {
            throw new InvalidOperationException("Cancellation is not allowed for this transaction.");
        }

        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException("Order not exist");
        }

        if (refundAmount > transaction.NetAmount)
        {
            throw new InvalidOperationException("Cancel amount exceeds the net amount of the transaction.");
        }

        var bankGateway = _gatewayFactory.Resolve(bank);

        var (isCancelSuccessful, bankReference) = await bankGateway.CancelAsync(order.OrderNumber, refundAmount, cancellationToken);

        var status = isCancelSuccessful ? TransactionStatuses.Success : TransactionStatuses.Fail;

        //stok geri eklemem gerekebilir eğer successe

        transaction.SetNetAmount(refundAmount, TransactionTypes.Refund);
        transaction.AddDetail(status: status, amount: refundAmount, type: TransactionTypes.Refund);

        await _transactionRepository.SaveChangesAsync(cancellationToken);
    }
}