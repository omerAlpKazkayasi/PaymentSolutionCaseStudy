using PaymentTestCase.Domain.Common;

namespace PaymentTestCase.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; }

    protected Product() { }

    public Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetPrice(price);
    }

    public void SetName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(Name), "Product name cannot be empty.");
        if (value.Length > 50)
            throw new ArgumentException("Product name cannot exceed 50 characters.", nameof(Name));
        Name = value;
    }

    public void SetPrice(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Product price cannot be negative.", nameof(Price));
        Price = value;
    }
}