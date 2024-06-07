using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class UserSessionPermission
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public DateTime Created { get; set; }

        public User User { get; set; }
        public Permission Permission { get; set; }

        public UserSessionPermission()
        {
            User = new User();
            Permission = new Permission();
        }
    }
}
