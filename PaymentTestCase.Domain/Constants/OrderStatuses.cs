namespace PaymentTestCase.Domain.Constants;

public static class OrderStatuses
{
    public const string Waiting = nameof(Waiting); 
    public const string Paid = nameof(Paid);
    public const string Cancelled = nameof(Cancelled);

    public static readonly IReadOnlySet<string> All = new HashSet<string>
    {
        Waiting,
        Paid,
        Cancelled
    };
}