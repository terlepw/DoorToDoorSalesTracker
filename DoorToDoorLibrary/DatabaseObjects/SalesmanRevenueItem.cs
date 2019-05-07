using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class SalesmanRevenueItem : BaseItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
