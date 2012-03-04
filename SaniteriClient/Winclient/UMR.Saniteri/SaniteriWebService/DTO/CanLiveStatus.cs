using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaniteriWebService.DTO
{
    public class CanLiveStatus
    {
        public Int64 CanID { get; set; }
        public bool? NeedService { get; set; }
        public bool? LidOpen { get; set; }
        public bool? DoorOpen { get; set; }
        public string Fault { get; set; }
        public double? Weight { get; set; }
        public string BagInfo { get; set; }
        public byte? PowerStatus { get; set; }
        public byte? CommunicationStatus { get; set; }
    }
}