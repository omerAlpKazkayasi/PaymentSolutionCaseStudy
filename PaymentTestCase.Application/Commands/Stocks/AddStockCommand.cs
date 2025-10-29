using MediatR;

namespace PaymentTestCase.Application.Commands.Stocks;

public record AddStockCommand(Guid productId, int quantity) : IRequest;