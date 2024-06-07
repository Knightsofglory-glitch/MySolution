using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Jon.Util.Status
{
    public class HTTPStatus
    {
        public int Status;
        public Severity Severity;


        public HTTPStatus(int httpStatus)
        {
            this.Status = httpStatus;
            initialize();
        }


        private void initialize()
        {
            if (this.Status < 300)
            {
                this.Severity = Severity.Success;
            }
            else if (this.Status < 400)
            {
                this.Severity = Severity.Warning;
            }
            else if (this.Status < 500)
            {
                this.Severity = Severity.Error;
            }
            else if (this.Status < 400)
            {
                this.Severity = Severity.Fatal;
            }
            else
            {
                this.Severity = Severity.Unknown;
            }
        }
    }
}
