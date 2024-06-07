using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class UserGroup
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }


        public UserGroup()
        {
            Group = new Group();
            User = new User();
        }
    }
}
