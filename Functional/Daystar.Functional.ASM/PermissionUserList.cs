using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class PermissionUserList
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public Permission Permission { get; set; }
        public List<User> UserList { get; set; }
        public DateTime Created { get; set; }


        public PermissionUserList()
        {
            Permission = new Permission();
            UserList = new List<User>();
        }
    }
}
