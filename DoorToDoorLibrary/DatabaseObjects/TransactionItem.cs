using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class TransactionItem : BaseItem
    {
        public DateTime Date { get; set; }
        public string House { get; set; }
        public string Product { get; set; }
        public double Amount { get; set; }
    }
}
