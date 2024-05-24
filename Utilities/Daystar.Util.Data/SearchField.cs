using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daystar.Util.Data
{
    public class SearchField
    {
        public string Name { get; set; }
        public Conjunction Conjunction { get; set; }
        public SearchOperation SearchOperation { get; set; }
        public Type Type { get; set; }
        public string Value { get; set; }


        public SearchField()
        {
            Conjunction = Conjunction.AND;
            SearchOperation = SearchOperation.Equal;
        }
        public SearchField(Conjunction conjunction)
        {
            Conjunction = conjunction;
            SearchOperation = SearchOperation.Equal;
        }
    }
}
