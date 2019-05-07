using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class HouseSalesCountItem : BaseItem
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public int SalesCount { get; set; }
    }
}
