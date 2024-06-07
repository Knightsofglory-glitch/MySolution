using System;
using System.Collections.Generic;

#nullable disable

namespace Jon.Services.Dbio.ASM
{
    public partial class GroupPermission
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int PermissionId { get; set; }
        public int? ApplicationId { get; set; }
        public DateTime Created { get; set; }
    }
}
