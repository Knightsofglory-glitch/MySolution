using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class GroupUserList
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public Group Group { get; set; }
        public List<User> UserList { get; set; }
        public DateTime Created { get; set; }

        public GroupUserList()
        {
            Group = new Group();
            UserList = new List<User>();
        }
    }
}
