using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Entity
{
    public abstract class AuditedEntity : Entity
    {
        public DateTime CreationTime { get; set; }
        public int? CreatorId { get; set; }

        public DateTime? LastModificationTime { get; set; }
        public int? LastModifierId { get; set; }
    }
}
