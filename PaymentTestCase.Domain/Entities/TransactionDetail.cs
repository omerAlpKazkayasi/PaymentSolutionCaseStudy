using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Domain.Entities;

public class TransactionDetail
{    
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public Transaction? Transaction { get; set; }
}