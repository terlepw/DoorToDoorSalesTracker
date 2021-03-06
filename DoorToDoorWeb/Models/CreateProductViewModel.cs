﻿using DoorToDoorLibrary.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class CreateProductViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = ErrorConsts.BlankError)]
        [MaxLength(ErrorConsts.MaxCharFifty, ErrorMessage = ErrorConsts.MaxFiftyError)]
        public string Name { get; set; }
    }
}
