using System;


namespace Jon.Functional.ASM
{
    public class vwUsersList
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public bool IsLogSession { get; set; }
        public DateTime Created { get; set; }
    }
}
