using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Bank)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(t => t.NetAmount)
            .HasPrecision(18, 2);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(t => t.TransactionDetails)
            .WithOne()
            .HasForeignKey(d => d.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}