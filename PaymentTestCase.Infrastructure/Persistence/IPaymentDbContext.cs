using Microsoft.EntityFrameworkCore;
using PaymentTestCase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Infrastructure.Persistence;

public interface IPaymentDbContext
{
    DbSet<Transaction> Transactions { get; }

    DbSet<TransactionDetail> TransactionDetails { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}