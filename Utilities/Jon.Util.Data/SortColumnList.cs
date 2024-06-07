using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jon.Util.Data
{
    public class SortColumnList
    {
        public List<SortColumn> Columns { get; set; }

        public SortColumnList()
        {
            Columns = new List<SortColumn>();
        }
    }
}
