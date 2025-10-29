using PaymentTestCase.Application.DTOs;

namespace PaymentTestCase.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAsync(Guid? id, string? name, CancellationToken cancellationToken);

    Task AddAsync(string name, decimal price, CancellationToken cancellationToken);
}