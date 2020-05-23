using CustomerTrackingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Models
{
    public class BaseViewModel
    {
        public string PageTitle { get; set; }
        public UserModel OnlineUser { get; set; }
    }
}
