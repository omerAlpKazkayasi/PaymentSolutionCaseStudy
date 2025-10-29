using MediatR;
using PaymentTestCase.Application.Commands.Product;
using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Application.Commands.Orders.Handlers;

public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand>
{
    private readonly IOrderService _orderService;

    public AddOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        await _orderService.AddAsync(request.Items, cancellationToken);
    }
}