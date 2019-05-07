using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public UpdateProfileViewModel UpdateProfile { get; set; }
        public SelfResetPasswordViewModel ResetPassword { get; set; }
    }
}
