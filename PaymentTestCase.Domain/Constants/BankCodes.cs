namespace PaymentTestCase.Domain.Constants;

public static class BankCodes
{
    public const string Akbank = nameof(Akbank);
    public const string Garanti = nameof(Garanti);
    public const string YapiKredi = nameof(YapiKredi);

    public static readonly IReadOnlySet<string> All =
        new HashSet<string> { Akbank, Garanti, YapiKredi };
}