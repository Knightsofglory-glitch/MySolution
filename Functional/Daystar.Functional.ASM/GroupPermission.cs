using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class GroupPermission
    {
        public int Id { get; set; }
        public Permission Permission { get; set; }
        public Group Group { get; set; }
        public int? ApplicationId { get; set; }
        public DateTime Created { get; set; }


        public GroupPermission()
        {
            Permission = new Permission();
            Group = new Group();
        }
    }
}
