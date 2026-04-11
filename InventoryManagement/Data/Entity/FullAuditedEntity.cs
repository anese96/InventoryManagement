using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Entity
{
    public abstract class FullAuditedEntity : AuditedEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string? DeleterId { get; set; }
    }
}
