using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Domain.Constants;

public class TransactionTypes
{
    public const string Sale = nameof(Sale);
    public const string Refund = nameof(Refund);
    public const string Cancel = nameof(Cancel);

    public static readonly IReadOnlySet<string> All =
        new HashSet<string>
        {
                Sale,
                Refund,
                Cancel
        };
}