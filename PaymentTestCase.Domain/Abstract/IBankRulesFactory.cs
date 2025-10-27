using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Domain.Abstract;

public interface IBankRulesFactory
{
    IBankRules Resolve(string bank);
}