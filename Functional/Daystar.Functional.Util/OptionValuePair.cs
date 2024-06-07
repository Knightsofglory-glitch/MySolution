using System;


namespace Jon.Functional.Util 
{
    public class OptionValuePair
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Image { get; set; }
        public bool bSelected { get; set; }

        public OptionValuePair()
        {
        }
        public OptionValuePair(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}
