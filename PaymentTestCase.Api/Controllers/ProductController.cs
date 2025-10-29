using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentTestCase.Application.Commands.Product;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Application.Queries.Products;

namespace PaymentTestCase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task Add([FromBody] AddProductCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
    }
}