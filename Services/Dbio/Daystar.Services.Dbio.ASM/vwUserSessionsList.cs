using System;
using System.Collections.Generic;

#nullable disable

namespace Jon.Services.Dbio.ASM
{
    public partial class vwUserSessionsList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime LastAction { get; set; }
        public DateTime Created { get; set; }
        public bool? IsLoggedOut { get; set; }
        public bool? IsTimedOut { get; set; }
        public DateTime? Terminated { get; set; }
    }
}
