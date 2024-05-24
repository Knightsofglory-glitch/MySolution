using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Daystar.Util.Data
{
    public enum SearchOperation
    {
        None = -1,
        Equal = 1,
        NotEquals = 2,
        LessThan = 3,
        LessThanOrEqual = 4,
        GreaterThan = 5,
        GreaterThanOrEqual = 6,
        BeginsWith = 7,
        DoesNotBeginWith = 8,
        Contains = 9,
        DoesNotContain = 10,
        EndsWith = 11,
        DoesNotEndWith = 12,
        IsNull = 13,
        IsNotNull = 14,
        DateOnly = 15,
        DateAndTime = 16
    }
}
