using AutoMapper;
using MediatR;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Constants;
using PaymentTestCase.Domain.Entities;
using System.Linq.Expressions;

namespace PaymentTestCase.Application.Services;

public class OrderService : IOrderService
{
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IProductRepository productRepository,
        IStockRepository stockRepository,
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(IList<OrderItemDto> items, CancellationToken cancellationToken)
    {
        var productIds = items.Select(item => item.ProductId).ToList();

        var isProductsExist = await _productRepository.ExistsAsync(p => productIds.Contains(p.Id));

        if (!isProductsExist)
        {
            throw new InvalidOperationException("One or more products do not exist.");
        }
        var orderNumber = 1;

        var lastOrder = (await _orderRepository.GetAsync(
            cancellationToken: cancellationToken))
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefault();

        if (lastOrder is not null)
        {
            orderNumber = lastOrder.OrderNumber + 1;
        }

        var order = new Order(orderNumber: orderNumber, status: OrderStatuses.Waiting);

        var stocks = await _stockRepository.GetAsync(
            s => productIds.Contains(s.ProductId),
            cancellationToken);

        var stocksDict = stocks.ToDictionary(s => s.ProductId);

        if (stocks is null || stocks.Count == 0)
        {
            throw new InvalidOperationException("One or more stocks do not exist.");
        }

        foreach (var item in items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);

            if (product is not null)
            {
                if (stocksDict.TryGetValue(item.ProductId, out var stock))
                {
                    if (stock.Quantity < item.Quantity)
                    {
                        throw new InvalidOperationException($"There is noo stock for product ID {item.ProductId}.");
                    }

                    order.AddItem(product, item.Quantity);
                }
                else
                {
                    continue;
                }
            }
            else
            {
                continue;
            }
        }

        await _orderRepository.AddAsync(order, cancellationToken);
    }

    public async Task<IEnumerable<OrderDto>> GetAsync(CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAsync(predicate: null, cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}