using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daystar.Util.Status;


namespace Daystar.Util.Data
{
    interface ILogger
    {
        daystarStatus LogException(System.Exception ex);
    }
}
