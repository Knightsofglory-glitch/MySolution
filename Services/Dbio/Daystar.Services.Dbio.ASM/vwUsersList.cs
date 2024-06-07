using System;
using System.Collections.Generic;

#nullable disable

namespace Jon.Services.Dbio.ASM
{
    public partial class vwUsersList
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public bool IsLogSession { get; set; }
        public bool IsDeleted { get; set; }
        public int? ApplicationId { get; set; }
        public DateTime Created { get; set; }
    }
}
