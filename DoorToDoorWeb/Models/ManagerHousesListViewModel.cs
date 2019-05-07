using DoorToDoorLibrary.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ManagerHousesListViewModel
    {
        public IList<HouseItem> Houses { get; set; }
        public CreateHouseViewModel CreatedHouse { get; set; }
        public IList<SelectListItem> PossibleSalespeople { get; set; }
    }
}
