using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI.Dashboard
{
    public class kpiAujd
    {
        private readonly AppDbContext _appDbContext;

        public kpiAujd(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public decimal CAAujourdhui(){  return 1;}
        public async Task<decimal> VentesAujourdhui()
        {
            var today = DateTime.Today;
            decimal total = await _appDbContext.SalesInvoices
                .Where(s => s.DateInvoice.Date == today)
                .SumAsync(s => (decimal?)s.TotalInvoice) ?? 0;

            return total;
        }
        public async Task<decimal> MargeBénéficiaireAujourdhui(){

            var today = DateTime.Today;
            decimal totalCoute = await _appDbContext.salesInvoiceLines
                .Where(s => s.SalesInvoice.DateInvoice.Date == today)
                .SumAsync(s => (decimal?)s.Quantity * s.PurchasePrice+ (Convert.ToDecimal(s.Taxe)*s.Quantity*s.PurchasePrice) ) ?? 0;

            return (CAAujourdhui()-totalCoute);
        }
}
}
