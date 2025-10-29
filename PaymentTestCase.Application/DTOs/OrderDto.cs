namespace PaymentTestCase.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }

    public string Status{ get; set; }

    public decimal TotalAmount { get; set; }
}