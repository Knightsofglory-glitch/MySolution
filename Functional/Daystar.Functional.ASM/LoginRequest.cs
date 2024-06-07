using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM  
{
    public class LoginRequest
    {
        public int ApplicationId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public bool bRememberMe { get; set; }

        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
