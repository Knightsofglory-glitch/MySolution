using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daystar.Util.Data
{
    public class SearchOptions
    {
        public List<SearchField> SearchFieldList { get; set; }
        public string SearchString { get; set; }
        public SearchOptions()
        {
            SearchFieldList = new List<SearchField>();
            SearchString = "";
        }
    }
}
