using PaymentTestCase.Domain.Common;

namespace PaymentTestCase.Domain.Entities;

public class Stock : BaseEntity
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    protected Stock() { }

    public Stock(Guid productId, int quantity)
    {
        Id = Guid.NewGuid();
        SetProductId(productId);
        SetQuantity(quantity);
    }

    public void SetProductId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ProductId cannot be empty.", nameof(ProductId));
        ProductId = value;
    }

    public void SetQuantity(int value)
    {
        if (value < 0)
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(Quantity));
        Quantity = value;
    }

    public void Increase(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Increase amount must be greater than zero.", nameof(amount));
        Quantity += amount;
    }

    public void Decrease(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Decrease amount must be greater than zero.", nameof(amount));
        if (Quantity - amount < 0)
            throw new InvalidOperationException("Insufficient stock to decrease.");
        Quantity -= amount;
    }
}
