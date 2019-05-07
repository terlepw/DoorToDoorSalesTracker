using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class UserItem : BaseItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public int RoleId { get; set; }
        public bool UpdatePassword { get; set; }

        public UserItem Clone()
        {
            UserItem item = new UserItem();
            item.Id = this.Id;
            item.FirstName = this.FirstName;
            item.LastName = this.LastName;
            item.EmailAddress = this.EmailAddress;
            item.Hash = this.Hash;
            item.Salt = this.Salt;
            item.RoleId = this.RoleId;
            item.UpdatePassword = this.UpdatePassword;
            return item;
        }
    }
}
