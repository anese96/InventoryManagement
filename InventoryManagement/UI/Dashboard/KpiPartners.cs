using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI.Dashboard
{
    public class KpiPartners
    {
        private readonly AppDbContext _appDbContext;

        public KpiPartners(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<decimal> TotalClients()
        {
            decimal total = await _appDbContext.Customers.CountAsync();
            return total;
        }
        public async Task<decimal> TotalFournisseurs()
        {
            decimal total = await _appDbContext.Vendors.CountAsync();
            return total;
        }
        public async Task<decimal> TotalSoldesClients()
        {
            decimal total = (decimal)await _appDbContext.Customers.SumAsync(c => c.Balance);
            return total;
        }
        public async Task<decimal> TotalSoldesFournisseurs()
        {
            decimal total = (decimal)await _appDbContext.Vendors.SumAsync(v => v.Balance);
            return total;
        }

    }
}
