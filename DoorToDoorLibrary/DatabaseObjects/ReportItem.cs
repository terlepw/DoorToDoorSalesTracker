using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class ReportItem : BaseItem
    {
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string ProductName { get; set; }
        public string SalespersonName { get; set; }
    }
}
