using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Services;

public class StockService : IStockService
{
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;

    public StockService(
        IProductRepository productRepository,
        IStockRepository stockRepository)
    {
        _productRepository = productRepository;
        _stockRepository = stockRepository;
    }

    public async Task AddAsync(Guid productId, int quantity, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAsync(x => x.Id == productId, cancellationToken);

        if (product is null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        var stock = new Stock(productId: productId, quantity: quantity);

        await _stockRepository.AddAsync(stock, cancellationToken);
    }
}