using Microsoft.EntityFrameworkCore;
using PaymentTestCase.Domain.Entities;
using PaymentTestCase.Infrastructure.Persistence;

namespace PaymentIntegration.Infrastructure.Persistence
{
    public class PaymentDbContext : DbContext, IPaymentDbContext
    {
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionDetail> TransactionDetails => Set<TransactionDetail>();

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }
    }
}