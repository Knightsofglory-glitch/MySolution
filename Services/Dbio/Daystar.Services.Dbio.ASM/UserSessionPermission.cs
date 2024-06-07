using System;
using System.Collections.Generic;

#nullable disable

namespace Jon.Services.Dbio.ASM
{
    public partial class UserSessionPermission
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public int PermissionId { get; set; }
        public DateTime Created { get; set; }
    }
}
