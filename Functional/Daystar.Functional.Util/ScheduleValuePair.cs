using System;
using System.Collections.Generic;
using System.Text;


namespace Jon.Functional.Util
{
     public  class ScheduleValuePair
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ScheduleValuePair()
        {
        }
        public ScheduleValuePair(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
