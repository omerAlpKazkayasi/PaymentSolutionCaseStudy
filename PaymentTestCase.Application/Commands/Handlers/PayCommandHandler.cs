using MediatR;
using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Commands.Handlers;

public class PayCommandHandler : IRequestHandler<PayCommand, Transaction>
{
    private readonly IPaymentService _paymentService;

    public PayCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<Transaction> Handle(PayCommand request, CancellationToken cancellationToken)
    {
        var result = await _paymentService.PayAsync(request.bank, request.orderReference, request.amount, cancellationToken);

        return result;
    }
}