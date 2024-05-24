using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Daystar.Util.Status
{
    public class StatusMessage
    {
        private uint _id;
        private string _acronym;
        private string _text;

        public StatusMessage(uint id, string acronym, string text)
        {
            _id = id;
            _acronym = acronym;
            _text = text;
        }

        public uint Id
        {
            get
            {
                return (_id);
            }
        }

        public string Acronym
        {
            get
            {
                return (_acronym);
            }
        }

        public string Text
        {
            get
            {
                return (_text);
            }
        }
    }
}
