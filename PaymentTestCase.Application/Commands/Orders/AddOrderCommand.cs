using MediatR;
using PaymentTestCase.Application.DTOs;

namespace PaymentTestCase.Application.Commands.Orders;

public class AddOrderCommand : IRequest
{
    public required IList<OrderItemDto> Items { get; set; }
}