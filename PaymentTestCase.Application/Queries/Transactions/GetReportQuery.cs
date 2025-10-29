using MediatR;
using PaymentTestCase.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.Queries.Transactions;

public class GetReportQuery : IRequest<IEnumerable<TransactionDto>>
{
    public Guid? TransactionId { get; set; }
    public Guid? OrderId { get; set; }
    public string? Bank { get; set; }
    public string? Status { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}