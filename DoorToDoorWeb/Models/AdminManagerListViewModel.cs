using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class AdminManagerListViewModel
    {
        public IList<UserItem> Managers { get; set; }
        public RegisterViewModel Register { get; set; }
    }
}
