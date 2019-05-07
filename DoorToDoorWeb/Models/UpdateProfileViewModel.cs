using DoorToDoorLibrary.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class UpdateProfileViewModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharHundred, ErrorMessage = ErrorConsts.MaxHundredError)]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        public string Password { get; set; }
    }
}
