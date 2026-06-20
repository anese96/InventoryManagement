using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
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
        public async Task<decimal> StockTotal()
        {
            int count = await _appDbContext.Products.CountAsync();
            if (count == 0)
                return 0;
            return count;
        }
        public async Task<decimal> ValeurStock()
        {
            decimal total = await _appDbContext.Products.
                SumAsync(p => (decimal?)p.StockQuantity * p.PurchasePrice) ?? 0;
            return total;
        }
        public decimal RuptureStock(){  return 1;}
        public decimal FaibleStock(){  return 1; }
    }
}
