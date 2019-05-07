using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class HouseItem : BaseItem
    {
        private static Dictionary<int, string> _statuses = new Dictionary<int, string>()
        {
            {1, "To Be Visited" },
            {2, "Contacted by Phone" },
            {3, "Contacted in Person" },
            {4, "Interested" },
            {5, "Not Interested" }
        };

        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public int ManagerID { get; set; }
        public int AssignedSalespersonID { get; set; }
        public int StatusID { get; set; }
        public string Status
        {
            get
            {
                return _statuses[this.StatusID];
            }
        }
        public string AssignedSalesperson { get; set; }
    }
}
