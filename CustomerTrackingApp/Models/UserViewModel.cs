using CustomerTrackingApp.Enum;
using CustomerTrackingApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CustomerTrackingApp.Models
{

    public class UserViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public Gender Gender { get; set; }
        public Enum.Type Type { get; set; }
        public int BirthYear { get; set; }
        public int ManagerId { get; set; }
    }
}
