using DoorToDoorLibrary.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class CreateHouseViewModel
    {
        [Display(Name = "Street")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string Street { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string City { get; set; }

        [Display(Name = "State/District")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string District { get; set; }

        [Display(Name = "Zip/Postal Code")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharTen, ErrorMessage = ErrorConsts.MaxTenError)]
        public string ZipCode { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string Country { get; set; }

        [Display(Name = "Assigned Salesperson")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        public int AssignedSalespersonID { get; set; }
    }
}
