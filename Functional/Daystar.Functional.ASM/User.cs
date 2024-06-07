using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class User
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password {get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public bool IsLogSession { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
    }
}
