using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.UI
{
    public class GetTotal
    {
        private readonly AppDbContext _appDbContext;


        public GetTotal(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<string> GetTotalProductCount()
        {
           
                int count = await _appDbContext.Products.CountAsync();
            if (count == 0)
                return "0";
            return count.ToString();
        }
        public async Task<string> GetTotaClientCount() 
        {
            int count = await _appDbContext.Customers.CountAsync();
            if (count == 0)
                return "0";
            return count.ToString();
        }
        public async Task<string> GetTotalVents()
        {
            DateTime debutMois = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime debutMoisSuivant = debutMois.AddMonths(1);

            double total = await _appDbContext.SalesInvoices
                .Where(c => c.DateInvoice >= debutMois &&
                            c.DateInvoice < debutMoisSuivant)
                .CountAsync();

            return total.ToString();
        }
        public async Task<string> GetTotalAchats()
        {
            DateTime debutMois = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime debutMoisSuivant = debutMois.AddMonths(1);

            double total = await _appDbContext.Purchases
                .Where(c => c.DatePurchase >= debutMois &&
                            c.DatePurchase < debutMoisSuivant)
                .CountAsync();

            return total.ToString();
        }
    }
}
