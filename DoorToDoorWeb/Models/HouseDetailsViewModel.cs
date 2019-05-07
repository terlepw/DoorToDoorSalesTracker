using DoorToDoorLibrary.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class HouseDetailsViewModel
    {
        public HouseItem House { get; set; }
        public IList<NoteItem> Notes { get; set; }
        public AddHouseNoteViewModel AddNote { get; set; }
        public IList<SelectListItem> StatusOptions { get; set; }
        public IList<ResidentItem> Residents { get; set; }
        public AddResidentViewModel AddResident { get; set; }
        public IList<SelectListItem> SalespersonOptions { get; set; }
    }
}
