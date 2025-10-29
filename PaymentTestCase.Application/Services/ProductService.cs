using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Entities;
using PaymentTestCase.Application.DTOs;
using System.Linq.Expressions;
using AutoMapper;

namespace PaymentTestCase.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(string name, decimal price, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var product = new Product(name: name, price: price);

            await _productRepository.AddAsync(product, cancellationToken);
        }
    }

    public async Task<IEnumerable<ProductDto>> GetAsync(Guid? id, string? name, CancellationToken cancellationToken)
    {

        Expression<Func<Product, bool>> predicate = p =>
            (string.IsNullOrEmpty(name) || p.Name.Contains(name)) &&
            (!id.HasValue || p.Id == id.Value);

        var products = await _productRepository.GetAsync(predicate: predicate, cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}