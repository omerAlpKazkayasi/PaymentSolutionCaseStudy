using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentTestCase.Application.Commands.Stocks;

namespace PaymentTestCase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task Add([FromBody] AddStockCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
    }
}