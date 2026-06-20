using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI.Dashboard
{
    public class KpiFinance
    {
        private readonly AppDbContext _appDbContext;

        public KpiFinance(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<decimal> TotalChiffreAffaires()
        {
            return 1;
        }
        public async Task<decimal> BeneficeDuMois()
        { 

            return 1;
        }
        public async Task<decimal> VentesDuMois()
        {
            DateTime debutMois = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime debutMoisSuivant = debutMois.AddMonths(1);

            double total = await _appDbContext.SalesInvoices
                .Where(c => c.DateInvoice >= debutMois &&
                            c.DateInvoice < debutMoisSuivant)
                .CountAsync();

            return (decimal)total;
        }
        public async Task<decimal> AchatsDuMois(){  return 1;}
    }
}
