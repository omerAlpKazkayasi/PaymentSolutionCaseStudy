using MediatR;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Commands;

public record PayCommand(string bank, string orderReference, decimal amount) : IRequest<Transaction>;