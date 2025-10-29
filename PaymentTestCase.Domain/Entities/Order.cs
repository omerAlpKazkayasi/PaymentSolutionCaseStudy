using PaymentTestCase.Domain.Common;
using PaymentTestCase.Domain.Constants;

namespace PaymentTestCase.Domain.Entities;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _items = new();

    public int OrderNumber { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = OrderStatuses.Waiting;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    protected Order() { }

    public Order(int orderNumber, string status)
    {
        SetOrderNumber(orderNumber);
        SetStatus(status);
        CreatedAt = DateTime.UtcNow;
    }

    public void SetOrderNumber(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Order number must be greater than zero.", nameof(OrderNumber));

        OrderNumber = value;
    }

    public void SetStatus(string value)
    {
        if (!OrderStatuses.All.Contains(value))
            throw new ArgumentException($"Invalid order status: {value}", nameof(Status));

        Status = value;
    }

    private void RecalculateTotal()
        => TotalAmount = _items.Sum(i => i.Quantity * i.UnitPrice);

    public void AddItem(Product product, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        var item = new OrderItem(Guid.NewGuid(), Id, product.Id, quantity, product.Price);
        _items.Add(item);
        RecalculateTotal();
    }
}

