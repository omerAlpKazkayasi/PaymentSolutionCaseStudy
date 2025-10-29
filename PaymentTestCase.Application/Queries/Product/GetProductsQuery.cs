using MediatR;
using PaymentTestCase.Application.DTOs;

namespace PaymentTestCase.Application.Queries.Products;

public record GetProductsQuery(Guid? id, string? name) : IRequest<IEnumerable<ProductDto>>;