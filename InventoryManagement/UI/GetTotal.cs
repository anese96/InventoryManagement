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
    }
}
