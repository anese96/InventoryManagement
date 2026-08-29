using DocumentFormat.OpenXml.Office2010.Excel;
using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Repositorys
{
    public class CratesRepository
    {
        private readonly AppDbContext _appContext;

        public CratesRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }

        public async Task AddMoney(int IdCrate ,decimal Totale)
        {
            var crate = await _appContext.Crates.FindAsync(IdCrate);
            var crateTotale = crate.Totale;
            if (crate == null)
            {
                throw new ArgumentException("Caisse not found.");
            }
            else
            {
                crate.Totale = crateTotale  + Totale;
                await _appContext.SaveChangesAsync();
            }
        }
    }
}
