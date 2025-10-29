using PaymentTestCase.Domain.Common;

namespace PaymentTestCase.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public int? LeftQuantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    protected OrderItem() { }

    public OrderItem(Guid orderId, Guid productId, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        SetOrderId(orderId);
        SetProductId(productId);
        SetQuantity(quantity);
        SetUnitPrice(unitPrice);
    }

    public void SetOrderId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("OrderId cannot be empty.", nameof(OrderId));
        OrderId = value;
    }

    public void SetProductId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ProductId cannot be empty.", nameof(ProductId));
        ProductId = value;
    }

    public void SetQuantity(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(Quantity));
        Quantity = value;
    }

    public void SetLeftQuantity(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(LeftQuantity));
        LeftQuantity = value;
    }

    public void SetUnitPrice(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(UnitPrice));
        UnitPrice = value;
    }
}