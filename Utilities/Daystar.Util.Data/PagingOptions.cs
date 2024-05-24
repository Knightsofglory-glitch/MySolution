using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daystar.Util.Data
{
    public class PagingOptions
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        public PagingOptions()
        {
            PageNumber = 1;
            PageSize = 100;
        }
    }
}
