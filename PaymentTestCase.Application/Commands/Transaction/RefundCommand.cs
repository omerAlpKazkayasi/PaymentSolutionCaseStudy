using MediatR;

namespace PaymentTestCase.Application.Commands.Transaction;

public record RefundCommand(Guid orderId, decimal amount) : IRequest;