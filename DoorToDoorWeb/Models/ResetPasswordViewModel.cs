using DoorToDoorLibrary.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ResetPasswordViewModel
    {
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [Compare("Password", ErrorMessage = ErrorConsts.NoPasswordMatch)]
        public string ConfirmPassword { get; set; }
    }
}
