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

        var productIds = order.Items.Select(i => i.ProductId).ToList();

        var stocks = await _stockRepository.GetAsync(s => productIds.Contains(s.ProductId), cancellationToken);

        foreach (var item in order.Items)
        {
            var stockItem = stocks.FirstOrDefault(s => s.ProductId == item.ProductId);

            if (stockItem is null)
            {
                throw new InvalidOperationException($"There is no stock data for {item.ProductId}");
            }
            else
            {
                if (stockItem.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException($"there are no {item.Quantity} stock for product {item.ProductId}");
                }
                else
                {
                    stockItem.Decrease(item.Quantity);

                    await _stockRepository.UpdateAsync(stockItem.Id, stockItem, cancellationToken);
                }
            }
        }

        var bankGateway = _gatewayFactory.Resolve(bank);

        var (isPaymentSuccessful, bankTransactionReference) = await bankGateway.PayAsync(order.OrderNumber, order.TotalAmount, cancellationToken);

        var status = isPaymentSuccessful ? TransactionStatuses.Success : TransactionStatuses.Fail;

        if (isPaymentSuccessful)
        {
            foreach (var item in order.Items)
            {
                var stockItem = stocks.First(s => s.ProductId == item.ProductId);
                stockItem.Decrease(item.Quantity);
                await _stockRepository.UpdateAsync(stockItem.Id, stockItem, cancellationToken);
            }
        }

        var newTransaction = new Transaction(
            orderId: orderId,
            bank: bank,
            totalAmount: order.TotalAmount,
            netAmount: order.TotalAmount,
            type: TransactionTypes.Sale,
            status: status);

        newTransaction.AddDetail(TransactionTypes.Sale, newTransaction.TotalAmount, status);

        await _transactionRepository.AddAsync(newTransaction, cancellationToken);
        await _transactionRepository.SaveChangesAsync(cancellationToken);
        await _stockRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelAsync(Guid orderId, decimal cancelAmount, CancellationToken cancellationToken)
    {
        var existingTransaction = await _transactionRepository.GetTransactionWithTransactionDetails(
            orderId,
            cancellationToken);

        if (existingTransaction == null)
        {
            throw new KeyNotFoundException("Transaction not found.");
        }

        var transaction = existingTransaction;
        var bank = transaction.Bank;

        var bankRules = _rulesFactory.Resolve(bank);

        if (!bankRules.CanCancel(transaction, DateTimeOffset.UtcNow))
        {
            throw new InvalidOperationException("Cancel transaction cant handle becasue day its not the same day with sale.");
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
        var existingTransaction = await _transactionRepository.GetTransactionWithTransactionDetails(
            orderId,
            cancellationToken);

        if (existingTransaction == null)
        {
            throw new KeyNotFoundException("Transaction not found.");
        }

        var transaction = existingTransaction;
        var bank = transaction.Bank;

        var bankRules = _rulesFactory.Resolve(bank);

        if (!bankRules.CanRefund(transaction, DateTimeOffset.UtcNow))
        {
            throw new InvalidOperationException("Cannot be returned because it has not even been one day");
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