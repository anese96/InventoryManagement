using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.DTO
{
    public class ReturnSalesLineDto
    {
        public int IdReturnSales { get; set; }
        public string RefProduct { get; set; }
        public string Designation { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string Taxe { get; set; }
        public decimal TotalWithoutTax { get; set; }

    }
}
