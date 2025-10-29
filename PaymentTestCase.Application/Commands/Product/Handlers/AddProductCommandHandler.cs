using MediatR;
using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Application.Commands.Product.Handlers;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
{
    private readonly IProductService _productService;

    public AddProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        await _productService.AddAsync(request.name, request.price, cancellationToken);
    }
}