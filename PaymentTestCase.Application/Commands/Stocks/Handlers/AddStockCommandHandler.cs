using MediatR;
using PaymentTestCase.Application.Commands.Product;
using PaymentTestCase.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.Commands.Stocks.Handlers;

class AddStockCommandHandler : IRequestHandler<AddStockCommand>
{
    private readonly IStockService _stockService;

    public AddStockCommandHandler(IStockService stockService)
    {
        _stockService = stockService;
    }

    public async Task Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        await _stockService.AddAsync(request.productId, request.quantity, cancellationToken);
    }
}