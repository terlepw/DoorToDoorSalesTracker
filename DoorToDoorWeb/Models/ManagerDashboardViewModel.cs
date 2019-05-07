using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ManagerDashboardViewModel
    {
        public IList<SalesmanSalesCountItem> SalesmanRankByQuantity { get; set; }
        public IList<SalesmanRevenueItem> SalesmanRankByRevenue { get; set; }
        public IList<HouseSalesCountItem> HouseRankByQuantity { get; set; }
        public IList<HouseRevenueItem> HouseRankByRevenue { get; set; }
        public int TotalSales { get; set; }
        public double TotalRevenue { get; set; }
    }
}
