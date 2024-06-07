using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Data;


namespace Jon.Functional.ASM
{
    public class UserSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public Application Application { get; set; }
        public List<Permission> Permissions { get; set; }
        public string Language { get; set; }
        public DateTime LastAction { get; set; }
        public DateTime Created { get; set; }
        public bool IsLoggedOut { get; set; }
        public bool IsTimedOut { get; set; }
        public DateTime? Terminated { get; set; }
        public string MachineId { get; set; }
        public string UserData1 { get; set; }


        public UserSession()
        {
            User = new User();
            Permissions = new List<Permission>();
        }
    }
}
