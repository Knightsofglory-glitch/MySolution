﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class UserGroupList
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public User User { get; set; }
        public List<Group> GroupList { get; set; }
        public DateTime Created { get; set; }

        public UserGroupList()
        {
            User = new User();
            GroupList = new List<Group>();
        }
    }
}
