using CustomerTrackingApp.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CustomerTrackingApp.Entities
{

    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public Gender Gender { get; set; }
        public Enum.Type Type { get; set; }
        public int BirthYear { get; set; }
        public IsActive IsActive { get; set; }
        public int ManagerId { get; set; }
        public UserModel(User user)
        {
            this.Id = user.Id;
            this.Username = user.Username;
            this.FullName = user.FullName;
            this.Email = user.Email;
            this.Phone = user.Phone;
            this.Gender = user.Gender;
            this.Type = user.Type;
            this.BirthYear = user.BirthYear;
            this.IsActive = user.IsActive;
            this.ManagerId = user.ManagerId;
        }
    }
}
