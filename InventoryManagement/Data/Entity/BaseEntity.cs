using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Entity
{
    public abstract class BaseEntity : Entity
    {
        public string Name { get; set; }    
    }
}
