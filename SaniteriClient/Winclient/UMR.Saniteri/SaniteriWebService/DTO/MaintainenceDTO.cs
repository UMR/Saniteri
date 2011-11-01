using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaniteriWebService.DTO
{
    public class MaintenanceDTO
    {
        public Int64 CanId { get; set; }
        public DateTime ServiceDate { get; set; }
        public String ServicePerformed { get; set; }
    }
}