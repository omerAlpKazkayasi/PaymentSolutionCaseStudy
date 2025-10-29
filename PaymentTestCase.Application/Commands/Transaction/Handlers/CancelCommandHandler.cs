using MediatR;
using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Application.Commands.Transaction.Handlers;

public class CancelCommandHandler : IRequestHandler<CancelCommand>
{
    private readonly IPaymentService _paymentService;

    public CancelCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task Handle(CancelCommand request, CancellationToken cancellationToken)
    {
        await _paymentService.CancelAsync(request.orderId, request.productId, request.quantity, cancellationToken);
    }
}