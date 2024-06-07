using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jon.Util.Data
{
    public class BaseId
    {
        public static int INVALID_ID = -1;
        public static int VALID_ID = 1;

        public int Id { get; set; }

        public BaseId()
        {
            Id = INVALID_ID;
        }
        public BaseId(int id)
        {
            Id = id;
        }
    }
}
