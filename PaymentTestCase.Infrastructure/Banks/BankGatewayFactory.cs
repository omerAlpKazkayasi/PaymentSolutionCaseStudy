using Microsoft.Extensions.DependencyInjection;
using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Infrastructure.Banks;

public sealed class BankGatewayFactory : IBankGatewayFactory
{
    private readonly IServiceProvider _provider;

    public BankGatewayFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IBankGateway Resolve(string bankId)
        => bankId switch
        {
            BankCodes.Akbank => _provider.GetRequiredService<AkbankGateway>(),
            BankCodes.Garanti => _provider.GetRequiredService<GarantiGateway>(),
            BankCodes.YapiKredi => _provider.GetRequiredService<YapiKrediGateway>(),
            _ => throw new KeyNotFoundException($"Unsupported bankId: {bankId}")
        };
}