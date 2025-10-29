using MediatR;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Application.Queries.Products;

namespace PaymentTestCase.Application.Queries.Order.Handlers;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IOrderService _orderService;

    public GetOrdersQueryHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAsync(cancellationToken);

        return orders;
    }
}