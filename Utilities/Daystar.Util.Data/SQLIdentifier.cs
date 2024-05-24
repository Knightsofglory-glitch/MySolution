using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Daystar.Util.Data
{
    public static class SQLIdentifier
    {

        public static void ParseThreePartIdentifier(string identifier, out string schema, out string entityName, out string columnName)
        {
            string _identifier = identifier.Replace("[", "").Replace("]", "");
            string[] _parts = _identifier.Split('.');
            schema = _parts[0];
            entityName = _parts[1];
            columnName = _parts[2];
        }
    }
}
