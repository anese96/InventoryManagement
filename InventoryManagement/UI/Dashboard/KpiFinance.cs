using InventoryManagement.Data;
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

        public decimal TotalChiffreAffaires(){  return 1;}
        public decimal BeneficeDuMois(){  return 1;}
        public decimal VentesDuMois(){  return 1;}
        public decimal AchatsDuMois(){  return 1;}
    }
}
