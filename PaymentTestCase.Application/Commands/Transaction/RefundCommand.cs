using MediatR;

namespace PaymentTestCase.Application.Commands.Transaction;

public record RefundCommand(Guid orderId, Guid productId, int quantity) : IRequest;