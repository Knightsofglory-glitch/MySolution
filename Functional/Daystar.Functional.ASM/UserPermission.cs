using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class UserPermission
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public User User { get; set; }
        public Permission Permission { get; set; }
        public DateTime Created { get; set; }


        public UserPermission()
        {
            User = new User();
            Permission = new Permission();
        }
    }
}
