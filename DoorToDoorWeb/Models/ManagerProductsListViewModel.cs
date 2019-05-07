using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ManagerProductsListViewModel
    {
        public IList<ProductItem> Products { get; set; }
        public CreateProductViewModel CreatedProduct { get; set; }
    }
}
