using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Status;


namespace Jon.Util.Data
{
    interface ILogger
    {
        jonStatus LogException(System.Exception ex);
    }
}
