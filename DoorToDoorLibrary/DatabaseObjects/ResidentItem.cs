using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class ResidentItem : BaseItem
    {
        public int HouseID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
