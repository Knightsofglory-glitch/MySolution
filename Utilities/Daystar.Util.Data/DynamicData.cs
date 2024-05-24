using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daystar.Util.Data
{
    public class DynamicData
    {
        public object Id { get; set; }
        public string Name { get; set; }
        public int NumRows { get; set; }
        public Dictionary<string, DataColumn> Columns = null;
        public List<dynamic> Data = null;


        public DynamicData()
        {
            NumRows = -1;
            Columns = new Dictionary<string, DataColumn>();
            Data = new List<dynamic>();
        }
        public DynamicData(object id)
        {
            Id = id;
            NumRows = -1;
            Columns = new Dictionary<string, DataColumn>();
            Data = new List<dynamic>();
        }

        public DataColumn GetColumn(string name)
        {
            DataColumn dataColumn = null;
            this.Columns.TryGetValue(name, out dataColumn);
            return (dataColumn);
        }
        public DataColumn GetColumn(int index)
        {
            return(this.Columns.Values.ElementAt(index));
        }
    }
}