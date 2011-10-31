using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaniteriWebService.DTO
{
    public class CanStatus
    {
        public Guid CanId { get; set; }
        public int? StatusType{get;set;}
        public DateTime? eDate { get; set;}
        public string StatusDescription { get; set;}               
    }
}