using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class PermissionUser
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public Permission Permission { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }


        public PermissionUser()
        {
            Permission = new Permission();
            User = new User();
        }
    }
}
