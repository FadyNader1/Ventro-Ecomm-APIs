using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities
{
    public class Photo:BaseEntity
    {
        public string ImageName { get; set; } = default!;
        public int ProductId { get; set; }
        //public virtual Product Product { get; set; } = default!;
    }
}
