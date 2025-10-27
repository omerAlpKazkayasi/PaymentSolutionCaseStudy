using Microsoft.Extensions.DependencyInjection;
using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Constants;
using PaymentTestCase.Domain.Rules;

namespace PaymentTestCase.Infrastructure.Banks;

public class BankRulesFactory : IBankRulesFactory
{
    private readonly IServiceProvider _serviceProvider;

    public BankRulesFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IBankRules Resolve(string bankId)
        => bankId switch
        {
            BankCodes.Akbank => _serviceProvider.GetRequiredService<AkbankRules>(),
            BankCodes.Garanti => _serviceProvider.GetRequiredService<GarantiRules>(),
            BankCodes.YapiKredi => _serviceProvider.GetRequiredService<YapiKrediRules>(),
            _ => throw new KeyNotFoundException($"Unsupported bank rules for: {bankId}")
        };
}