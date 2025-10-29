using MediatR;

namespace PaymentTestCase.Application.Commands.Transaction;

public record PayCommand(string bank, Guid orderId) : IRequest;