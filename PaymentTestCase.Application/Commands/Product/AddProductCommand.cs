using MediatR;

namespace PaymentTestCase.Application.Commands.Product;

public record AddProductCommand(string name, int price) : IRequest;