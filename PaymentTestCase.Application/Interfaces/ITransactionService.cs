using PaymentTestCase.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetReportAsync(
        Guid? transactionId,
        Guid? orderId,
        string? bank,
        string? status,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken);
}