using System;


namespace Jon.Functional.Util
{
    public class Document
    {
        public int Id { get; set; }
        public bool IsSynched { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Contents { get; set; }
        public long Size { get; set; }
        public int? ServerId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }

        public string ExportFilename { get; set; }
    }
}
