using MediatR;

namespace PaymentTestCase.Application.Commands.Transaction;

public record CancelCommand(string bank, string orderReference, decimal amount) : IRequest;