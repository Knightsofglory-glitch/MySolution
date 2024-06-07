using System;
using System.Collections.Generic;

#nullable disable

namespace Jon.Services.Dbio.ASM
{
    public partial class vwUserPermissionsList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public int? ApplicationId { get; set; }
        public DateTime Created { get; set; }
    }
}
