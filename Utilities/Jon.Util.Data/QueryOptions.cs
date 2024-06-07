using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jon.Util.Data
{
    public class QueryOptions
    {
        public SortColumnList SortColumns { get; set; }
        public PagingOptions Paging { get; set; }
        public SearchOptions SearchOptions { get; set; }

        public QueryOptions()
        {
            SortColumns = new SortColumnList();
            Paging = new PagingOptions();
            SearchOptions = new SearchOptions();
        }
        public QueryOptions(int pageSize, int pageNumber)
            :this()
        {
            this.Paging.PageSize = pageSize;
            this.Paging.PageNumber = pageNumber;
        }
    }
}
