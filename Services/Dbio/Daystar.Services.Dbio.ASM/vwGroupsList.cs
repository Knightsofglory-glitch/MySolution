using System;
using System.Collections.Generic;

#nullable disable

namespace Jon.Services.Dbio.ASM
{
    public partial class vwGroupsList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
