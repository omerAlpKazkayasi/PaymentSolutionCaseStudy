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

    public async Task CancelAsync(Guid orderId, Guid productId, int quantity, CancellationToken cancellationToken)
    {
        await ProcessTransactionAsync(
            orderId,
            productId,
            quantity,
            TransactionTypes.Cancel,
            cancellationToken);
    }

    public async Task RefundAsync(Guid orderId, Guid productId, int quantity, CancellationToken cancellationToken)
    {
        await ProcessTransactionAsync(
            orderId,
            productId,
            quantity,
            TransactionTypes.Refund,
            cancellationToken);
    }

    private async Task ProcessTransactionAsync(
    Guid orderId,
    Guid productId,
    int quantity,
    string transactionType,
    CancellationToken cancellationToken)
    {
        var transaction = await ValidateAndGetTransaction(orderId, transactionType, cancellationToken);

        if (transaction == null)
        {
            throw new InvalidOperationException("Transaction validation failed.");
        }

        var (order, orderItem, amount) = await ValidateAndGetOrderData(
            orderId,
            productId,
            quantity,
            transaction.NetAmount,
            cancellationToken);

        var (isSuccessful, bankReference) = await ExecuteBankOperation(
            transaction.Bank,
            order.OrderNumber,
            amount,
            transactionType,
            cancellationToken);

        await UpdateAfterSuccessfulOperation(
            productId,
            quantity,
            orderItem,
            transaction,
            amount,
            transactionType,
            isSuccessful,
            cancellationToken);
    }

    private async Task<Transaction> ValidateAndGetTransaction(
        Guid orderId,
        string transactionType,
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetTransactionWithTransactionDetails(
            orderId,
            cancellationToken);

        if (transaction == null)
        {
            return null;
        }

        var bankRules = _rulesFactory.Resolve(transaction.Bank);

        if (transactionType == TransactionTypes.Cancel)
        {
            if (!bankRules.CanCancel(transaction, DateTimeOffset.UtcNow))
            {
                return null;
            }
        }
        else if (transactionType == TransactionTypes.Refund)
        {
            if (!bankRules.CanRefund(transaction, DateTimeOffset.UtcNow))
            {
                return null;
            }
        }

        return transaction;
    }

    private async Task<(Order order, OrderItem orderItem, decimal amount)> ValidateAndGetOrderData(
        Guid orderId,
        Guid productId,
        int quantity,
        decimal transactionNetAmount,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithOrderItems(orderId, cancellationToken);

        if (order is null || order.Items.Count == 0)
        {
            throw new InvalidOperationException("Order not exist");
        }

        var orderItem = order.Items.FirstOrDefault(i => i.ProductId == productId);

        if (orderItem is null)
        {
            throw new InvalidOperationException("Order item not exist");
        }

        if (orderItem.LeftQuantity is not null && orderItem.LeftQuantity - quantity < 0)
        {
            throw new InvalidOperationException("Order item not exist");
        }

        var amount = orderItem.UnitPrice * quantity;

        if (amount > transactionNetAmount)
        {
            throw new InvalidOperationException(
                "Cancel amount exceeds the net amount of the transaction.");
        }

        return (order, orderItem, amount);
    }

    private async Task<(bool isSuccessful, string bankReference)> ExecuteBankOperation(
        string bank,
        int orderNumber,
        decimal amount,
        string transactionType,
        CancellationToken cancellationToken)
    {
        var bankGateway = _gatewayFactory.Resolve(bank);

        if (transactionType == TransactionTypes.Cancel)
        {
            return await bankGateway.CancelAsync(orderNumber, amount, cancellationToken);
        }
        else
        {
            return await bankGateway.RefundAsync(orderNumber, amount, cancellationToken);
        }
    }

    private async Task UpdateAfterSuccessfulOperation(
        Guid productId,
        int quantity,
        OrderItem orderItem,
        Transaction transaction,
        decimal amount,
        string transactionType,
        bool isSuccessful,
        CancellationToken cancellationToken)
    {
        var status = isSuccessful ? TransactionStatuses.Success : TransactionStatuses.Fail;

        if (isSuccessful)
        {
            var stock = await _stockRepository.GetAsync(
                s => s.ProductId == productId,
                cancellationToken);

            if (stock is not null && stock.Count > 0)
            {
                stock[0].Increase(quantity);
            }

            if (orderItem.LeftQuantity is null)
            {
                orderItem.SetLeftQuantity(orderItem.Quantity - quantity);
            }
            else
            {
                orderItem.SetLeftQuantity(orderItem.LeftQuantity.Value - quantity);
            }

            transaction.SetNetAmount(amount, transactionType);
        }

        transaction.AddDetail(status: status, amount: amount, type: transactionType);

        await _transactionRepository.SaveChangesAsync(cancellationToken);
    }
}