using DoorToDoorLibrary.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class TransactionsViewModel
    {
        public IList<TransactionItem> Transactions { get; set; }
        public AddTransactionViewModel AddTransaction { get; set; }
        public IList<SelectListItem> HouseList { get; set; }
        public IList<SelectListItem> ProductList { get; set; }
    }
}
