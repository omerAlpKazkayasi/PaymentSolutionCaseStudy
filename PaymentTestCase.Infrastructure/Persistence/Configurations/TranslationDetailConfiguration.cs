using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Infrastructure.Persistence.Configurations;

public class TranslationDetailConfiguration : IEntityTypeConfiguration<TransactionDetail>
{
    public void Configure(EntityTypeBuilder<TransactionDetail> builder)
    {
        builder.ToTable("TransactionDetails");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.TransactionType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Amount)
            .HasPrecision(18, 2);
    }
}