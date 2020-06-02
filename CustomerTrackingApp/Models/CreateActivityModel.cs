using CustomerTrackingApp.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Models
{
    public class CreateActivityModel
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentDebt { get; set; }
    }
}
