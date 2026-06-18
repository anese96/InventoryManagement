using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI.Dashboard
{
    public class kpiStock
    {
        private readonly AppDbContext _appDbContext;
        public kpiStock(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public decimal StockTotal(){  return 1;}
        public decimal ValeurStock(){  return 1;}
        public decimal RuptureStock(){  return 1;}
        public decimal FaibleStock(){  return 1; }
    }
}
