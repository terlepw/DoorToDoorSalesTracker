using DoorToDoorLibrary.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class AddHouseNoteViewModel
    {
        public int HouseID { get; set; }

        [Display(Name = "Note:")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxChar255, ErrorMessage = ErrorConsts.Max255Error)]
        public string Note { get; set; }
    }
}
