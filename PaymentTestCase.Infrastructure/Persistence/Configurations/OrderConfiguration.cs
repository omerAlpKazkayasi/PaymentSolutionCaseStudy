using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentTestCase.Domain.Entities;

namespace PaymentTestCase.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasOne<Transaction>()
            .WithOne(t => t.Order)
            .HasForeignKey<Transaction>(t => t.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}