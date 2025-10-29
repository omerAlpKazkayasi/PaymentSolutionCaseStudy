using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.DTOs;

public class TransactionDto
{
    public required string Bank { get; set; }
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public required string Status { get; set; }

    public required IList<TransactionDetailDto> TransactionDetails { get; set; }
}