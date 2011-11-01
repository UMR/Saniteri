using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaniteriWebService.DTO
{
    public class InventoryDTO
    {
       public Int64 CanId { get; set; }
       public DateTime? ProductionDate { get; set; }
       public DateTime? InServiceDate { get; set; }
       public String Street { get; set; }
       public String Additional { get; set;}
       public String City { get; set; }
       public String State { get; set; }
       public String Zip { get; set; }
       public String Floor { get; set; }
       public String Room { get; set; }
       public String Custom1 { get; set; }
       public String Custom2 { get; set; }
       public String Custom3 { get; set; }
       public String IpAddress { get; set; } 
    }
}