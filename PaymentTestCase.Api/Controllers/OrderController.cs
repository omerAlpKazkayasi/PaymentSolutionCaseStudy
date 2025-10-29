using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentTestCase.Application.Commands.Orders;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Application.Queries.Order;

namespace PaymentTestCase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task CreateOrder([FromBody] AddOrderCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrdersQuery(), cancellationToken);
        return Ok(result);
    }
}