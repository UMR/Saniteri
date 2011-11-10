using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaniteriWebService.DTO
{
    public class TransactionLogDTO
    {
        public Int64 CanId { get; set; }
        public DateTime EventTimeStamp { get; set; }
        public int? EventType { get; set; }
        public String EventDescription { get; set; }
        public String EventData { get; set; }  

    }
}