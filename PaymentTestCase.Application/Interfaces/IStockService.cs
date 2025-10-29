namespace PaymentTestCase.Application.Interfaces;

public interface IStockService
{
    Task AddAsync(Guid productId, int quantity, CancellationToken cancellationToken);
}