using DoorToDoorLibrary.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class SelfResetPasswordViewModel
    {
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [Compare("NewPassword", ErrorMessage = ErrorConsts.NoPasswordMatch)]
        public string ConfirmNewPassword { get; set; }
    }
}
