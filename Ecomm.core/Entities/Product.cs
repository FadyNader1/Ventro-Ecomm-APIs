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
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = default!;
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    }
}
