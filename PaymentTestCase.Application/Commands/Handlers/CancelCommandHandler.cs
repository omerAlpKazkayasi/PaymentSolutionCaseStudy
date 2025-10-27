using MediatR;
using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Application.Commands.Handlers;

public class CancelCommandHandler : IRequestHandler<CancelCommand, Transaction>
{
    private readonly IPaymentService _paymentService;

    public CancelCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<Transaction> Handle(CancelCommand request, CancellationToken cancellationToken)
    {
        var result = await _paymentService.CancelAsync(request.bank, request.orderReference, request.amount, cancellationToken);

        return result;
    }
}