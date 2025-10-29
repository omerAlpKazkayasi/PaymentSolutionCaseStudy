using MediatR;
using PaymentTestCase.Application.Interfaces;

namespace PaymentTestCase.Application.Commands.Transaction.Handlers;

public class RefundCommandHandler : IRequestHandler<RefundCommand>
{
    private readonly IPaymentService _paymentService;

    public RefundCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task Handle(RefundCommand request, CancellationToken cancellationToken)
    {
        await _paymentService.RefundAsync(request.orderId, request.productId ,request.quantity, cancellationToken);
    }
}