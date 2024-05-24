using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daystar.Util.Status;


namespace Daystar.Util.Data
{
    public class TypeBuilder
    {
        public daystarStatus BuildType(List<DataColumn> columnList, out Dictionary<string, DataColumn> dynamicType)
        {
            // Initialize
            dynamicType = new Dictionary<string, DataColumn>();
            List<string> columnNameList = new List<string>();


            foreach (DataColumn column in columnList)
            {
                // If key already exists, append with increasing integer values.
                string keyName = column.Name;
                List<string> duplicateKeyNames = columnNameList.FindAll(delegate (string k) { return k == keyName; });
                if (duplicateKeyNames.Count == 0)
                {
                    columnNameList.Add(keyName);
                }
                else
                {
                    keyName += columnNameList.Count.ToString();
                }

                dynamicType.Add(keyName, column);
            }
            return (new daystarStatus(Severity.Success));
        }
    }
}
