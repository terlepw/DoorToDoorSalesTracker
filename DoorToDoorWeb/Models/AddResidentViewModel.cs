using DoorToDoorLibrary.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class AddResidentViewModel
    {
        public int HouseID { get; set; }

        [Display(Name = "First Name:")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string LastName { get; set; }
    }
}
