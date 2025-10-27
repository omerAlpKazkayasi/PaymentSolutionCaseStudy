using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Domain.Constants;

public static class TransactionStatuses
{
    public const string Success = nameof(Success);
    public const string Fail = nameof(Fail);

    public static readonly IReadOnlySet<string> All =
        new HashSet<string>
        {
                Success,
                Fail
        };
}