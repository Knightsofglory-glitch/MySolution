using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jon.Functional.Util
{
    public class AGSettingText
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string LoveGiftMinimumAmount { get; set; }
        public string SegmentWebLink { get; set; }
        public string ShippingTimeframeMessage { get; set; }
        public int AGGreetingId { get; set; }
        public int AGSubGreetingId { get; set; }
        public int AGClosingId { get; set; }
        public int AGPrayerOnlyClosingId { get; set; }
        public string CreatedDateTime { get; set; }

        public bool IsText { get; set; }
        public string AGGreetingText { get; set; }
        public string AGSubGreetingText { get; set; }
        public string AGClosingText { get; set; }
        public string AGPrayerOnlyClosingText { get; set; }
    }
}
