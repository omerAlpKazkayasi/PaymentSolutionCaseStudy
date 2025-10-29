using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.DTOs;

public class TransactionDetailDto
{
    public Guid Id { get; set; }
    public Guid TransactionId { get;  set; }
    public required string TransactionType { get; set; }
    public required string Status { get;  set; }
    public decimal Amount { get;  set; }
}