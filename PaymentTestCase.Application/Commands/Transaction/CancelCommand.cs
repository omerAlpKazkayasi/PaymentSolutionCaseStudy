using MediatR;

namespace PaymentTestCase.Application.Commands.Transaction;

public record CancelCommand(Guid orderId, Guid productId, int quantity) : IRequest;