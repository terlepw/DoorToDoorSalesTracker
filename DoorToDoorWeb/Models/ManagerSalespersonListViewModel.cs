using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ManagerSalespersonListViewModel
    {
        public IList<UserItem> Salespeople { get; set; }
        public RegisterViewModel Register { get; set; }
    }
}
