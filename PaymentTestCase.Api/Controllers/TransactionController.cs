using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentTestCase.Application.Commands.Transaction;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Application.Queries.Transactions;

namespace PaymentTestCase.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TransactionController : Controller
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task Pay([FromBody] PayCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
    }

    [HttpPost]
    public async Task Cancel([FromBody] CancelCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
    }

    [HttpPost]
    public async Task Refund([FromBody] RefundCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetReport([FromQuery] GetReportQuery query, CancellationToken cancellationToken)
    {
        var  result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}