using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class PasswordPair
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime Modified { get; set; }
    }
}
