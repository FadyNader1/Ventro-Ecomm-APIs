using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? KeySpecs { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }

        public int InventoryQuantity { get; set; } //stock available
        public bool IsFeatured { get; set; } = false; 
        public bool IsNewArrival { get; set; } = true; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = default!;
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    }
}
