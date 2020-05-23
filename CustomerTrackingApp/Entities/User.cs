using CustomerTrackingApp.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CustomerTrackingApp.Entities
{
    
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public Gender Gender { get; set; }
        public Enum.Type Type { get; set; }
        public int BirthYear { get; set; }
        public IsActive IsActive { get; set; }
        public int ManagerId { get; set; }
    }
}
