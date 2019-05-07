using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class RoleItem : BaseItem
    {
        public string Name { get; set; }

        public RoleItem Clone()
        {
            RoleItem item = new RoleItem();
            item.Id = this.Id;
            item.Name = this.Name;
            return item;
        }
    }
}


