using PaymentTestCase.Domain.Abstract;

namespace PaymentTestCase.Domain.Common;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public bool IsDeleted => DeletedAt.HasValue;

    public void MarkUpdated() => UpdatedAt = DateTimeOffset.UtcNow;
    public void MarkDeleted() => DeletedAt = DateTimeOffset.UtcNow;
}