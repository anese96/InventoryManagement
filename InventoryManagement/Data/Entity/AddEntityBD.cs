using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Entity
{
    public class AddEntityBD 
    {
        public DateTime ECreationTime() {

           return DateTime.Now;
        }
        public int ECreatorId() {

           return 1;
        }
    }
}
