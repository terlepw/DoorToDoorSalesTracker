using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DatabaseObjects
{
    public class NoteItem : BaseItem
    {
        public int HouseID { get; set; }
        public int UserID { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
    }
}
