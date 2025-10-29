using MediatR;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Application.Queries.Products.Handlers;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductService _productService;

    public GetProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _productService.GetAsync(query.id, query.name, cancellationToken);

        return products;
    }
}