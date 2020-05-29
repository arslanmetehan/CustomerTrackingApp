using CustomerTrackingApp.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CustomerTrackingApp.Entities
{
    
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Phone { get; set; }
    }
}
