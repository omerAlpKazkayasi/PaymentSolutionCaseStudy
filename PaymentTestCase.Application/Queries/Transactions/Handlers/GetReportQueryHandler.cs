using AutoMapper;
using MediatR;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Abstract.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.Queries.Transactions.Handlers;

class GetReportQueryHandler : IRequestHandler<GetReportQuery, IEnumerable<TransactionDto>>
{
    private readonly ITransactionService _transactionService;

    public GetReportQueryHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<IEnumerable<TransactionDto>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        var report = await _transactionService.GetReportAsync(
            request.TransactionId, 
            request.OrderId, 
            request.Bank, 
            request.Status, 
            request.StartDate, 
            request.EndDate, 
            cancellationToken);

        return report;
    }
}