using AutoMapper;
using PaymentTestCase.Application.DTOs;
using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionDto>> GetReportAsync(
        Guid? transactionId,
        Guid? orderId,
        string? bank,
        string? status,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken)
    {
        Expression<Func<Transaction, bool>> predicate = p =>
            (string.IsNullOrEmpty(bank) || p.Bank == bank) &&
            (string.IsNullOrEmpty(status) || p.Status == status) &&
            (!orderId.HasValue || p.OrderId == orderId.Value) && 
            (!startDate.HasValue ||
                p.TransactionDate >= new DateTimeOffset(startDate.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)) &&
            (!endDate.HasValue ||
                p.TransactionDate <= new DateTimeOffset(endDate.Value.ToDateTime(TimeOnly.MaxValue), TimeSpan.Zero));

        var transactions = await _transactionRepository.GetWithDetailsAsync(predicate: predicate, cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}