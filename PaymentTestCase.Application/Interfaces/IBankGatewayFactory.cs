using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Application.Interfaces;

public interface IBankGatewayFactory
{
    IBankGateway Resolve(string bankId);
}