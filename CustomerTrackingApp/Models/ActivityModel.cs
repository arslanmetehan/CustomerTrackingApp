using CustomerTrackingApp.Enum;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CustomerTrackingApp.Entities
{

    public class ActivityModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentDebt { get; set; }
        public ActivityModel() { }
        public ActivityModel(Activity activity)
        {
            this.Id = activity.Id;
            this.UserId = activity.UserId;
            this.CustomerId = activity.CustomerId;
            this.ActivityType = activity.ActivityType;
            this.Description = activity.Description;
            this.Amount = activity.Amount;
            this.CurrentDebt = activity.CurrentDebt;
        }
    }
}
