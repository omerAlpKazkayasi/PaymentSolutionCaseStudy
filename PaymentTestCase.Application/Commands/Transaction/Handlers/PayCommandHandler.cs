using MediatR;
using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Application.Commands.Transaction.Handlers;

public class PayCommandHandler : IRequestHandler<PayCommand>
{
    private readonly IPaymentService _paymentService;

    public PayCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task Handle(PayCommand request, CancellationToken cancellationToken)
    {
        await _paymentService.PayAsync(request.bank, request.orderId, cancellationToken);
    }
}