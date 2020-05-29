using CustomerTrackingApp.Enum;
using CustomerTrackingApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CustomerTrackingApp.Models
{

    public class CustomerViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Phone { get; set; }
    }
}
